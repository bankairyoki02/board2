import React, { useState, useEffect } from 'react';
import {
  Box,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
  Chip,
  CircularProgress,
  Alert,
  FormControlLabel,
  Checkbox,
  TextField,
  Button,
} from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import dayjs, { Dayjs } from 'dayjs';
import { apiService, ScheduleBoardDto, ScheduleItemDto } from '../services/api';

const ScheduleBoard: React.FC = () => {
  const [scheduleData, setScheduleData] = useState<ScheduleBoardDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  // Filter states
  const [startDate, setStartDate] = useState<Dayjs | null>(dayjs().subtract(30, 'day'));
  const [endDate, setEndDate] = useState<Dayjs | null>(dayjs().add(90, 'day'));
  const [includeProposals, setIncludeProposals] = useState(true);
  const [showDetail, setShowDetail] = useState(true);
  const [showFuture, setShowFuture] = useState(true);
  const [selectedWarehouses, setSelectedWarehouses] = useState<string>('');

  const loadScheduleBoard = async () => {
    try {
      setLoading(true);
      setError(null);
      
      const params = {
        startDate: startDate?.format('YYYY-MM-DD'),
        endDate: endDate?.format('YYYY-MM-DD'),
        warehouses: selectedWarehouses,
        includeProposals,
        showDetail,
        showFuture,
      };
      
      const data = await apiService.getScheduleBoard(params);
      setScheduleData(data);
    } catch (err) {
      setError('Failed to load schedule board data');
      console.error('Error loading schedule board:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadScheduleBoard();
  }, []);

  const handleRefresh = () => {
    loadScheduleBoard();
  };

  const getStatusColor = (status: string, isLate: boolean, isProposal: boolean) => {
    if (isLate) return 'error';
    if (isProposal) return 'warning';
    
    switch (status.toLowerCase()) {
      case 'completed': return 'success';
      case 'in progress': return 'info';
      case 'planned': return 'default';
      default: return 'default';
    }
  };

  const formatDate = (dateString: string) => {
    return dayjs(dateString).format('MM/DD/YYYY');
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="400px">
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Alert severity="error" sx={{ m: 2 }}>
        {error}
        <Button onClick={handleRefresh} sx={{ ml: 2 }}>
          Retry
        </Button>
      </Alert>
    );
  }

  return (
    <LocalizationProvider dateAdapter={AdapterDayjs}>
      <Box sx={{ p: 3 }}>
        <Typography variant="h4" gutterBottom>
          Scheduling Board
        </Typography>
        
        {/* Filters */}
        <Paper sx={{ p: 2, mb: 3 }}>
          <Typography variant="h6" gutterBottom>
            Filters
          </Typography>
          <Box display="flex" flexDirection="column" gap={2}>
            <Box display="flex" gap={2} flexWrap="wrap">
              <Box sx={{ minWidth: 200 }}>
                <DatePicker
                  label="Start Date"
                  value={startDate}
                  onChange={setStartDate}
                  slotProps={{ textField: { fullWidth: true, size: 'small' } }}
                />
              </Box>
              <Box sx={{ minWidth: 200 }}>
                <DatePicker
                  label="End Date"
                  value={endDate}
                  onChange={setEndDate}
                  slotProps={{ textField: { fullWidth: true, size: 'small' } }}
                />
              </Box>
              <Box sx={{ minWidth: 250 }}>
                <TextField
                  label="Warehouses (comma-separated)"
                  value={selectedWarehouses}
                  onChange={(e) => setSelectedWarehouses(e.target.value)}
                  fullWidth
                  size="small"
                  placeholder="e.g., WH001,WH002"
                />
              </Box>
              <Button
                variant="contained"
                onClick={handleRefresh}
                sx={{ minWidth: 120 }}
              >
                Refresh
              </Button>
            </Box>
            <Box display="flex" gap={2} flexWrap="wrap">
              <FormControlLabel
                control={
                  <Checkbox
                    checked={includeProposals}
                    onChange={(e) => setIncludeProposals(e.target.checked)}
                  />
                }
                label="Include Proposals"
              />
              <FormControlLabel
                control={
                  <Checkbox
                    checked={showDetail}
                    onChange={(e) => setShowDetail(e.target.checked)}
                  />
                }
                label="Show Detail"
              />
              <FormControlLabel
                control={
                  <Checkbox
                    checked={showFuture}
                    onChange={(e) => setShowFuture(e.target.checked)}
                  />
                }
                label="Show Future"
              />
            </Box>
          </Box>
        </Paper>

        {/* Schedule Items Table */}
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Project</TableCell>
                <TableCell>Warehouse</TableCell>
                <TableCell>Part</TableCell>
                <TableCell>Scheduled Date</TableCell>
                <TableCell>Quantity</TableCell>
                <TableCell>Status</TableCell>
                <TableCell>Notes</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {scheduleData?.scheduleItems.map((item: ScheduleItemDto) => (
                <TableRow key={item.id}>
                  <TableCell>
                    <Box>
                      <Typography variant="body2" fontWeight="bold">
                        {item.projectNumber}
                      </Typography>
                      <Typography variant="caption" color="text.secondary">
                        {item.projectName}
                      </Typography>
                    </Box>
                  </TableCell>
                  <TableCell>
                    <Box>
                      <Typography variant="body2" fontWeight="bold">
                        {item.warehouseCode}
                      </Typography>
                      <Typography variant="caption" color="text.secondary">
                        {item.warehouseName}
                      </Typography>
                    </Box>
                  </TableCell>
                  <TableCell>
                    {item.partNumber ? (
                      <Box>
                        <Typography variant="body2" fontWeight="bold">
                          {item.partNumber}
                        </Typography>
                        <Typography variant="caption" color="text.secondary">
                          {item.partDescription}
                        </Typography>
                      </Box>
                    ) : (
                      <Typography variant="body2" color="text.secondary">
                        No part specified
                      </Typography>
                    )}
                  </TableCell>
                  <TableCell>
                    <Typography variant="body2">
                      {formatDate(item.scheduledDate)}
                    </Typography>
                  </TableCell>
                  <TableCell>
                    <Typography variant="body2">
                      {item.quantity.toLocaleString()}
                    </Typography>
                  </TableCell>
                  <TableCell>
                    <Chip
                      label={item.status}
                      color={getStatusColor(item.status, item.isLate, item.isProposal)}
                      size="small"
                    />
                    {item.isLate && (
                      <Chip
                        label="LATE"
                        color="error"
                        size="small"
                        sx={{ ml: 1 }}
                      />
                    )}
                    {item.isProposal && (
                      <Chip
                        label="PROPOSAL"
                        color="warning"
                        size="small"
                        sx={{ ml: 1 }}
                      />
                    )}
                  </TableCell>
                  <TableCell>
                    <Typography variant="body2" color="text.secondary">
                      {item.notes || '-'}
                    </Typography>
                  </TableCell>
                </TableRow>
              ))}
              {scheduleData?.scheduleItems.length === 0 && (
                <TableRow>
                  <TableCell colSpan={7} align="center">
                    <Typography variant="body2" color="text.secondary">
                      No schedule items found for the selected criteria
                    </Typography>
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>

        {/* Summary */}
        {scheduleData && (
          <Paper sx={{ p: 2, mt: 3 }}>
            <Typography variant="h6" gutterBottom>
              Summary
            </Typography>
            <Box display="flex" gap={4} flexWrap="wrap">
              <Box>
                <Typography variant="body2" color="text.secondary">
                  Total Items
                </Typography>
                <Typography variant="h6">
                  {scheduleData.scheduleItems.length}
                </Typography>
              </Box>
              <Box>
                <Typography variant="body2" color="text.secondary">
                  Late Items
                </Typography>
                <Typography variant="h6" color="error.main">
                  {scheduleData.scheduleItems.filter(item => item.isLate).length}
                </Typography>
              </Box>
              <Box>
                <Typography variant="body2" color="text.secondary">
                  Proposals
                </Typography>
                <Typography variant="h6" color="warning.main">
                  {scheduleData.scheduleItems.filter(item => item.isProposal).length}
                </Typography>
              </Box>
              <Box>
                <Typography variant="body2" color="text.secondary">
                  Active Projects
                </Typography>
                <Typography variant="h6">
                  {scheduleData.projects.filter(p => p.isActive).length}
                </Typography>
              </Box>
            </Box>
          </Paper>
        )}
      </Box>
    </LocalizationProvider>
  );
};

export default ScheduleBoard;