<p align="center">
  <img src="docs/logo.png" width="350">
</p>

<p align="center">
  <i>
    TriQue is a modern solution for TODA organizations seeking faster dispatching,<br>
    cleaner workflows, and smarter queue management.
  </i>
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


</div>
<div align="center">

[Overview](#-project-description-and-purpose) · [Features](#-features-and-functionalities) · [Architecture](#️-uml-diagram) · [Getting Started](#-how-to-run-the-application) · [Team](#-developers)

</div>

<br>

# 📖 Project Description and Purpose

**TriQue** is an automated terminal management solution that modernizes the operations of **Tricycle Operators and Drivers Associations (TODA)** in the Philippines. It replaces error-prone manual paper logs with a digital **First-In, First-Out (FIFO)** queuing system and an automated **Route Rotation** module.

Built with **C# WinForms** and an **Object-Oriented architecture**, TriQue organizes drivers into groups, assigns them to routes on a fair rotation schedule, and tracks every driver's status in real time — from joining the queue to completing a trip.


### 🎯 Key Objectives

| | Objective | Description |
|---|---|---|
| ⚖️ | **Operational Fairness** | Eliminates *"singitan"* (queue jumping) through a strict digital sequence, giving every driver an equal opportunity to earn |
| 🔄 | **Route Systematization** | Automates rotation of driver groups across destinations, ensuring high-traffic routes are shared equitably |
| 🔍 | **Transparency & Accountability** | Maintains a verifiable digital trail of all activities — trip durations, earnings, and terminal traffic |
| ⚡ | **Terminal Efficiency** | Reduces manual record-keeping for TODA officers so they can focus on order and safety |
<br>


# 🗂️ UML Diagram

<details>
  <summary>Show Model Class Diagram</summary>
   <br>
      <div align="center">
            <img src="docs/uml1.jpg" alt="Model Class Diagram" style="width:100%; max-width:800px; cursor:zoom-in;">
         </a>
      </div>
</details>
   <br>

<details>
  <summary>Report Module Class Diagram</summary>
   <br>
      <div align="center">
            <img src="docs/uml2.jpg" alt="Report Module Class Diagram" style="width:100%; max-width:800px; cursor:zoom-in;">
         </a>
      </div>
</details>
   <br>

**Architecture Pattern:**
```
🖥️ Forms  →  ⚙️ Services  →  🗄️ Repositories  →  💾 Database
```

# ✨ Features and Functionalities

From the queue to the road — here's everything TriQue can do:

| | Feature | For | Description |
|---|---|---|---|
| 📊 | **Dashboard** | Driver | Your day at a glance — earnings, trips, and how close you are to your goal. |
| 🎯 | **Earnings Goal** | Driver | Watch your progress bar go 🔴→🟠→🟢. Hit your target and the app celebrates with you. |
| 🔢 | **Queue** | Driver | Know exactly where you stand — join the queue and track your position live. |
| 🚦 | **Trip Control** | Driver | Tap to start, tap to end. Fare is calculated for you — no math needed. |
| 🗺️ | **Route Map** | Driver | See your route and know the traffic before you even leave the terminal. |
| 📜 | **Trip History** | Driver | A running record of your last 10 trips — where you went, what you earned. |
| 📈 | **Dashboard** | Admin | The whole terminal in one screen — who's moving, who's idle, and what's busy. |
| 🚦 | **Traffic Monitor** | Admin | Always knows which route is suffering and when — so you don't have to guess. |
| 👁️ | **Queue Viewer** | Admin | Every driver, every route, every position — live and always up to date. |
| 👥 | **User Management** | Admin | Full control over who's in the system and what they can do. *(SuperAdmin only)* |
| 📄 | **Reports** | Admin | Turn raw trip data into clean, exportable PDFs in seconds. |
| 🔒 | **Account Lockout** | System | Too many wrong attempts? The system locks the door and starts a countdown. |
| 🔑 | **Temporary Password** | System | First login, new password — no exceptions. |
| 📋 | **Auth Log** | System | Every entry and exit from the system, logged and timestamped. |

<br>

# ⚙️ How the Program Works

### 🔐 Authentication

1. User launches TriQue and is presented with the login screen.
2. User enters their username and password.
3. System verifies credentials and checks for account lockouts.
4. If using a temporary password, user is forced to set a new one before continuing.
5. User is redirected to their role-specific dashboard.


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

<br>


# 🚀 How to Run the Application
 
### 📋 Requirements
- 🪟 Windows 10 or later
- ⚙️ .NET 8.0 SDK or later
- 🛠️ Visual Studio 2022 (recommended)
- 🗺️ TomTom API Key — [get one free here](https://developer.tomtom.com) <br><br>


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

<br>

# 👥 Developers
From concept to code — meet the people who made TriQue possible.

| | Name | Role |
|:---:|:---|:---|
| [<img src="https://avatars.githubusercontent.com/u/140367080?v=4" width="45" height="45" style="border-radius:50%">](https://github.com/st-f4tima) | *Fatima A. Pura* | *Project Manager / Lead Developer* |
| [<img src="https://avatars.githubusercontent.com/u/111299443?v=4" width="45" height="45" style="border-radius:50%">](https://github.com/Artemissssssss) | *Matthew Louis G. Labrador* | *GUI Developer* |
| [<img src="https://avatars.githubusercontent.com/u/191230027?v=4" width="45" height="45" style="border-radius:50%">](https://github.com/philip696969) | *Philip Joshua D. Vinluan* | *Logic Developer* |
| [<img src="https://avatars.githubusercontent.com/u/191589293?v=4" width="45" height="45" style="border-radius:50%">](https://github.com/JamesMendozaRiniya) | *James Gabriel S. Mendoza* | *Quality Assurance / Tester* |

<br>

# 🌸 Acknowledgment

Building TriQue was a challenging but meaningful experience for our group, made possible through the guidance and support of the people who helped us throughout the process.

We sincerely thank our instructor, **Ms. Darlene Opeña**, for sharing her knowledge, guiding us through every stage of development, and pushing us beyond our limits with her feedback and encouragement.

Most of all, we are grateful to every member of the TriQue group for the effort, patience, and teamwork that brought this project to completion.

  — *TriQUE Group 💙*


