Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Public Class FileDecryption
    Private Shared Function GenerateKey(password As String, salt As Byte()) As Byte()
        Dim keyGenerator As New Rfc2898DeriveBytes(password, salt, 10000) ' 10,000 iterations
        Return keyGenerator.GetBytes(32) ' AES-256 requires a 32-byte key
    End Function

    Public Shared Function DecryptFile(encryptedFilePath As String, password As String) As String
        Try
            Dim encryptedBytes As Byte() = File.ReadAllBytes(encryptedFilePath)

            ' Extract salt (16 bytes) and IV (16 bytes)
            Dim salt As Byte() = encryptedBytes.Take(16).ToArray()
            Dim iv As Byte() = encryptedBytes.Skip(16).Take(16).ToArray()
            Dim data As Byte() = encryptedBytes.Skip(32).ToArray() ' Skip salt + IV

            ' Generate key using the same method used for encryption
            Dim key As Byte() = GenerateKey(password, salt)

            Using aes As Aes = Aes.Create()
                aes.Key = key
                aes.IV = iv
                aes.Padding = PaddingMode.PKCS7

                Using decryptor As ICryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV)
                    Dim decryptedBytes As Byte() = decryptor.TransformFinalBlock(data, 0, data.Length)
                    Return Encoding.UTF8.GetString(decryptedBytes)
                End Using
            End Using
        Catch ex As Exception
            Return $"Error: {ex.Message}"
        End Try
    End Function

End Class
