<p align="center">
  <img src="docs/logo.png" width="250">
</p>

<p align="center">
  <i align="center">TriQue is a modern solution for TODA organizations seeking faster dispatching, cleaner workflows, and smarter queue management.</i>
</p>

<div align="center">

![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET_8.0-3a0096?style=flat-square&logo=dotnet&logoColor=white)
![WinForms](https://img.shields.io/badge/Windows_Forms-0057b8?style=flat-square&logo=windows&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-0a9fa8?style=flat-square&logo=sqlite&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual_Studio_2022-7b2fbe?style=flat-square&logo=visualstudio&logoColor=white)
![Course](https://img.shields.io/badge/Course-Advanced%20OOP-e07b00?style=flat-square)
![Platform](https://img.shields.io/badge/Platform-Windows-0057b8?style=flat-square&logo=windows&logoColor=white)
![Contributors](https://img.shields.io/badge/Contributors-4-7b2fbe?style=flat-square&logo=github&logoColor=white)
![Year](https://img.shields.io/badge/Year-2026-0a9fa8?style=flat-square)

</div>
<div align="center">

[Overview](#-project-description-and-purpose) · [Features](#-features-and-functionalities) · [Architecture](#️-uml-diagram) · [Getting Started](#-how-to-run-the-application) · [Team](#-developers)

</div>

## 📖 Project Description and Purpose

**TriQue** is an automated terminal management solution that modernizes the operations of **Tricycle Operators and Drivers Associations (TODA)** in the Philippines. It replaces error-prone manual paper logs with a digital **First-In, First-Out (FIFO)** queuing system and an automated **Route Rotation** module.

Built with **C# WinForms** and an **Object-Oriented architecture**, TriQue organizes drivers into groups, assigns them to routes on a fair rotation schedule, and tracks every driver's status in real time — from joining the queue to completing a trip.


### 🎯 Key Objectives

| | Objective | Description |
|---|---|---|
| ⚖️ | **Operational Fairness** | Eliminates *"singitan"* (queue jumping) through a strict digital sequence, giving every driver an equal opportunity to earn |
| 🔄 | **Route Systematization** | Automates rotation of driver groups across destinations, ensuring high-traffic routes are shared equitably |
| 🔍 | **Transparency & Accountability** | Maintains a verifiable digital trail of all activities — trip durations, earnings, and terminal traffic |
| ⚡ | **Terminal Efficiency** | Reduces manual record-keeping for TODA officers so they can focus on order and safety |


## 🗂️ UML Diagram

<i>Will implement soon.</i>

**Architecture Pattern:**
```
🖥️ Forms  →  ⚙️ Services  →  🗄️ Repositories  →  💾 Database
```

## ✨ Features and Functionalities

###  Driver

| | Feature | Description |
|---|---|---|
| 📊 | **Driver Dashboard** | Displays daily earnings, completed trips, fastest and slowest trip stats, and a color-coded earnings goal progress bar that updates in real time. |
| 🎯 | **Earnings Goal Tracker** | Visual progress bar shifts from 🔴 red to 🟠 orange to 🟢 green as the driver approaches their daily goal. A congratulatory alert fires upon completion. |
| 🔢 | **Queue Management** | Drivers can join the queue for their assigned route. The system displays their current position and live status within the queue. |
| 🚦 | **Trip Control** | Drivers start and end trips directly from the queue view. The system automatically calculates the fare using the LTFRB tricycle fare formula. |
| 🗺️ | **Live Route Map** | TomTom-powered interactive map renders the driver's assigned route with real-time traffic status — Light, Moderate, or Heavy. |
| 📜 | **Trip History** | Displays the last 10 completed trips including route name, actual earnings, and trip date in a sortable data grid. |

---

### 🛡️ Admin

| | Feature | Description |
|---|---|---|
| 📈 | **Admin Dashboard** | Centralized overview showing total trips, highest and lowest trip routes, driver status distribution via pie chart, and drivers per route via bar graph. |
| 🚦 | **Traffic Monitor** | Identifies the most traffic-prone route and its peak congestion window using the TomTom API. Data auto-refreshes every 30 minutes. |
| 👁️ | **Queue Viewer** | Real-time view of all drivers currently in queue per route, including position, body number, and status. |
| 👥 | **User Management** | SuperAdmin-exclusive module for creating, editing, and managing driver and admin accounts across the system. |
| 📄 | **Report Generation** | Accessible by SuperAdmins and Toda Officers. Generates operational reports based on trip data and terminal activity. |

---

### 🔐 Authentication

| | Feature | Description |
|---|---|---|
| 🔒 | **Account Lockout** | Automatically locks an account after 3 consecutive failed login attempts, with a 1-minute cooldown before retry is allowed. |
| ⏱️ | **Live Countdown Timer** | During lockout, a real-time countdown is displayed on the login screen so the driver knows exactly when they can try again. |
| 🔑 | **Temporary Password** | New accounts are issued a temporary password. The system forces a mandatory password change on the very first login. |
| 📋 | **Auth Activity Log** | Every authentication event — successful login, failed attempt, lockout trigger, and logout — is recorded with a timestamp for accountability. |

---

### 👤 Access Levels

| | Role | Access Scope |
|---|---|---|
| 👑 | **SuperAdmin** | Full system access — user management, reports, queue view, and all admin features. |
| 📋 | **Toda Officer** | Access to report generation and queue monitoring. Cannot manage user accounts. |
| 🎛️ | **Staff** | Queue view only. Monitors driver status and route activity without administrative privileges. |

## ⚙️ How the Program Works

### 🔐 Authentication

1. User launches TriQue and is presented with the login screen.
2. User enters their username and password.
3. System verifies credentials and checks for account lockouts.
4. If using a temporary password, user is forced to set a new one before continuing.
5. User is redirected to their role-specific dashboard.

---

### 🛺 Driver

**Dashboard**
1. Driver lands on the dashboard after login.
2. Daily earnings, trip stats, and earnings goal progress are displayed.
3. An interactive map loads showing the assigned route with live traffic status.
4. Driver clicks **Join Queue** to enter the FIFO queue for their route.

**View Queue**
1. Driver sees their current position and status in the queue.
2. When it is their turn, the **Start Trip** button becomes active.
3. Driver clicks **Start Trip** — status updates to *On Trip* across the system.
4. Upon arrival, driver clicks **End Trip** — fare is automatically calculated.
5. Driver returns to *Waiting* status and may rejoin the queue for another trip.

**Settings**
1. Driver updates personal account information and preferences.

---

### 🛡️ Admin

**Dashboard**
1. Admin lands on the dashboard after login.
2. Total trips, route performance, and driver status charts are displayed.
3. Traffic data is fetched from the TomTom API and refreshes every 30 minutes.

**View Queue**
1. Admin selects a route to monitor.
2. Live queue is displayed — driver names, body numbers, positions, and statuses.

**Manage Users** *(SuperAdmin only)*
1. SuperAdmin navigates to Manage Users.
2. Admin accounts and driver accounts can be created, viewed, and managed.

**Generate Reports** *(SuperAdmin and Toda Officer)*
1. Authorized user navigates to Generate Reports.
2. System compiles trip history and terminal activity into a report.

**Settings**
1. Admin updates account details and system preferences.


## 🚀 How to Run the Application
 
 
### 📋 Requirements
- 🪟 Windows 10 or later
- ⚙️ .NET 8.0 SDK or later
- 🛠️ Visual Studio 2022 (recommended)
- 🗺️ TomTom API Key — [get one free here](https://developer.tomtom.com)
---
 
### 🛠️ Setup Steps
 
**1️⃣ Clone the repository**
```bash
git https://github.com/st-f4tima/TriQUE.git
cd triQUE
```
 
**2️⃣ Configure your TomTom API Key**
 
Create a `.env` file in the project root:
```
TOMTOM_API_KEY=your_api_key_here
```
 
**3️⃣ Configure default credentials**
 
Create `appsettings.Development.json` and set your preferred password defaults:
```json
{
  {
  "SeedPasswords": {
    "AdminDefault": "password_here",
    "DriverDefault": "password_here"
  }
}
}
```
 
**4️⃣ Open the solution**
 
Open `TriQue.slnx` in Visual Studio 2022.
 
**5️⃣ Restore NuGet packages**
```
Tools → NuGet Package Manager → Restore Packages
```
 
**6️⃣ Build and run**
```
Press F5 or click ▶ Start
```
 
> ✅ The SQLite database is created automatically on first run — no additional setup needed.
 
> ⚠️ All default accounts use temporary passwords — a mandatory password change is required on first login.
 

## 👥 Developers

| | Name | Role |
|:---:|:---|:---|
| [<img src="https://avatars.githubusercontent.com/u/140367080?v=4" width="45" height="45" style="border-radius:50%">](https://github.com/st-f4tima) | *Fatima A. Pura* | *Project Manager / Lead Developer* |
| [<img src="https://avatars.githubusercontent.com/u/111299443?v=4" width="45" height="45" style="border-radius:50%">](https://github.com/Artemissssssss) | *Matthew Louis G. Labrador* | *GUI Developer* |
| [<img src="https://avatars.githubusercontent.com/u/191230027?v=4" width="45" height="45" style="border-radius:50%">](https://github.com/philip696969) | *Philip Joshua D. Vinluan* | *Logic Developer* |
| [<img src="https://avatars.githubusercontent.com/u/191589293?v=4" width="45" height="45" style="border-radius:50%">](https://github.com/JamesMendozaRiniya) | *James Gabriel S. Mendoza* | *Quality Assurance / Tester* |

<h1 align="center">🌸 Acknowledgment 🌸</h1>

Building TriQue was a challenging but meaningful experience for our group, made possible through the guidance and support of the people who helped us throughout the process.

We sincerely thank our instructor, **Ms. Darlene Opeña**, for sharing her knowledge, guiding us through every stage of development, and pushing us beyond our limits with her feedback and encouragement.

Most of all, we are grateful to every member of the TriQue group for the effort, patience, and teamwork that brought this project to completion.
 
  — *Group 13 💙*


