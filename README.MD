# Real-Time System Resource Monitor

A simple `.NET 10` console application built to monitor basic system resources like **CPU usage, RAM usage, and Disk space** in real time. The application continuously displays system information in the terminal, stores logs in text files, and can also send the data to a REST API endpoint.

This project was created to practice:
- C# basics
- file handling
- background tasks
- REST API calls
- simple system monitoring

---

# 🚀 Getting Started

Make sure you have the [.NET 10 SDK](https://dotnet.microsoft.com/download) installed.

---

# 1. Clone the Project

```bash
git clone https://github.com/theshrish46/resource-monitor
cd resource-monitor
```

---

# 2. Configure Settings

Open the `appsettings.json` file and update the values if needed.

```json
{
  "MonitorSettings": {
    "IntervalSeconds": 3,
    "APIEndpoint": "https://webhook.site/your-endpoint"
  }
}
```

- `IntervalSeconds` → Time between each monitoring cycle
- `APIEndpoint` → API URL where the data will be sent

---

# 3. Run the Application

```bash
dotnet restore
dotnet build
dotnet run
```

Press `Ctrl + C` anytime to stop the application.

---

# 📊 Features

- Monitor CPU usage
- Monitor RAM usage
- Monitor Disk space
- Display live system stats in terminal
- Save logs into text files
- Send data to REST API endpoint

---

# 🛠️ Project Structure

```text
resource-monitor/
│
├── bin/
├── obj/
├── Models/
├   ├── SystemStat.cs
├── Monitor/
├   ├── IMetricCollector.cs
├   ├── LinuxMetricCollector.cs
├   ├── MaxMetricCollector.cs
├   ├── WindowsMetricCollector.cs
├── Services/
├   ├── IMonitorPlugin.cs
├   ├── ConsoleLoggerPlugin.cs
├   ├── FIleLoggerPlugin.cs
├   ├── HttpSenderPlugin.cs
├── appsettings.json
├── Program.cs
├── README.MD
```

---

# 📁 Logging

The application automatically creates a `Logs` folder and stores monitoring data inside log files.

Example:

```text
Logs/metrics_2026-05-24.log
```

---

# 🌐 API Payload Example

The application sends monitoring data in JSON format.

```json
{
  "cpu": 12.45,
  "ram_used": 4112.80,
  "disk_used": 154320.12
}
```

---

# ⚙️ How It Works

1. The application starts a background monitoring loop.
2. System resource information is collected based on the interval specified.
3. Data is displayed in the terminal.
4. The same data is written into log files.
5. Resource data is also sent to a REST API endpoint.

---

# 📚 Concepts Used

This project helped in learning:

- C# console applications
- Classes and objects
- Background tasks
- File handling
- JSON handling
- HTTP requests
- Basic logging
- REST API integration

---

# ✅ Notes

- Built using `.NET 10`
- Designed as a beginner-friendly system monitoring project
- Focused on learning core backend and monitoring concepts
