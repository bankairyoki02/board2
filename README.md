# Scheduling Board - Modernized Application

This project represents the modernization of a legacy VB.NET Windows Forms scheduling board application to a modern .NET 8 Web API backend with a React 18 frontend.

## Architecture Overview

### Backend (.NET 8)
- **Clean Architecture** with Domain, Application, Infrastructure, and API layers
- **CQRS Pattern** using MediatR for command/query separation
- **Entity Framework Core** with SQLite for data persistence
- **RESTful API** with Swagger documentation
- **Domain-Driven Design** with proper entity relationships

### Frontend (React 18)
- **Material-UI (MUI)** for modern, responsive UI components
- **TypeScript** for type safety and better development experience
- **Axios** for HTTP client communication
- **Date Pickers** for enhanced date selection
- **Responsive Design** that works on desktop and mobile

## Project Structure

```
modernized/
├── backend/
│   ├── SchedulingBoard.Domain/          # Domain entities and enums
│   ├── SchedulingBoard.Application/     # Business logic and CQRS
│   ├── SchedulingBoard.Infrastructure/  # Data access and external services
│   └── SchedulingBoard.API/            # Web API controllers and configuration
└── frontend/
    ├── src/
    │   ├── components/                  # React components
    │   ├── services/                    # API service layer
    │   └── pages/                       # Page components
    └── public/                          # Static assets
```

## Key Features Modernized

### From Legacy VB.NET to Modern Web
- **Windows Forms → React Components**: Replaced desktop UI with responsive web interface
- **MSFlexGrid → Material-UI Table**: Modern data grid with sorting and filtering
- **VB.NET → C# .NET 8**: Updated to latest .NET with modern C# features
- **Local Database → Web API**: Centralized data access through RESTful API
- **Desktop App → Web Application**: Accessible from any device with a browser

### Enhanced Functionality
- **Real-time Filtering**: Dynamic date range and warehouse filtering
- **Status Indicators**: Visual chips for late items, proposals, and status
- **Responsive Design**: Works on desktop, tablet, and mobile devices
- **Modern UI/UX**: Clean, intuitive interface with Material Design
- **API Documentation**: Swagger/OpenAPI documentation for developers

## Domain Model

### Core Entities
- **Warehouse**: Production facilities with location and operational details
- **Project**: Work orders with timeline and status tracking
- **Part**: Components with specifications and costs
- **ScheduleItem**: Individual scheduled work items linking projects, warehouses, and parts

### Key Relationships
- Projects contain multiple ScheduleItems
- ScheduleItems reference Warehouses and Parts
- Warehouses can be grouped for organizational purposes
- Parts can have references and qualifications

## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+ and npm
- Modern web browser

### Running the Application

1. **Start the Backend API**:
   ```bash
   cd backend/SchedulingBoard.API
   dotnet run --urls="http://0.0.0.0:5000"
   ```

2. **Start the Frontend**:
   ```bash
   cd frontend
   npm start
   ```

3. **Access the Application**:
   - Frontend: http://localhost:12000
   - API Documentation: http://localhost:5000/swagger

### Sample Data
The application automatically seeds sample data including:
- 2 Warehouses (Main Production Facility, Secondary Warehouse)
- 2 Projects (Q1 Production Run, Special Order Processing)
- 2 Parts (Standard and Premium Widget Components)
- 4 Schedule Items with various statuses and dates

## API Endpoints

### Schedule Board
- `GET /api/ScheduleBoard` - Get schedule data with filtering options
- Parameters: startDate, endDate, warehouses, includeProposals, showDetail, showFuture

### Warehouses
- `GET /api/Warehouses` - Get all warehouses
- `GET /api/Warehouses/groups` - Get warehouse groups

## Technology Stack

### Backend
- .NET 8
- Entity Framework Core 9.0.6
- MediatR 12.5.0
- SQLite Database
- Swagger/OpenAPI

### Frontend
- React 18
- TypeScript
- Material-UI (MUI)
- Axios for HTTP requests
- Day.js for date handling

## Migration Benefits

### Performance
- **Web-based**: No client installation required
- **Scalable**: Can handle multiple concurrent users
- **Cross-platform**: Works on Windows, Mac, Linux, mobile devices

### Maintainability
- **Clean Architecture**: Separation of concerns for easier maintenance
- **Modern Frameworks**: Active community support and regular updates
- **Type Safety**: TypeScript reduces runtime errors
- **API-First**: Backend can support multiple frontends

### User Experience
- **Responsive Design**: Adapts to different screen sizes
- **Modern UI**: Intuitive Material Design interface
- **Real-time Updates**: Dynamic filtering and data refresh
- **Accessibility**: Better support for screen readers and keyboard navigation

## Future Enhancements

### Planned Features
- **Authentication & Authorization**: User login and role-based access
- **Real-time Updates**: WebSocket integration for live data updates
- **Advanced Filtering**: More sophisticated search and filter options
- **Data Export**: Excel/PDF export functionality
- **Audit Trail**: Track changes and user actions
- **Mobile App**: Native mobile application using React Native

### Technical Improvements
- **Caching**: Redis integration for improved performance
- **Logging**: Structured logging with Serilog
- **Monitoring**: Application performance monitoring
- **Testing**: Comprehensive unit and integration tests
- **CI/CD**: Automated build and deployment pipelines

## Legacy System Comparison

| Aspect | Legacy VB.NET | Modernized Solution |
|--------|---------------|-------------------|
| Platform | Windows Desktop | Web Browser |
| UI Framework | Windows Forms | React + Material-UI |
| Data Access | Direct DB Access | RESTful API |
| Database | SQL Server | SQLite (easily changeable) |
| Deployment | MSI Installer | Web Deployment |
| Scalability | Single User | Multi-user |
| Maintenance | Monolithic | Modular Architecture |
| Testing | Manual | Automated Testing Ready |

This modernization transforms a legacy desktop application into a modern, scalable, and maintainable web application while preserving all core functionality and improving the user experience.