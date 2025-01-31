# Project Overview
The Finance Tracker is a hobby project currently in development, created to enhance personal financial management. It is designed to help users, including the developer, track expenses and subscriptions visually and effectively.

**Features**

* Track expenses with categories for better organization.
* Keep track of subscriptions to avoid unnecessary charges.
* Built using React Native for both Android and iOS platforms.
  
**Technology Stack**
* Frontend: React Native
* Backend: .NET
* Database: SQL Server
* User Authentication: JWT tokens and .NET Identity
* Middleware: ExpoGo
* Design Pattern: Repository Pattern

**Purpose**

The app is being developed to help individuals, particularly those who struggle with managing their finances, keep track of their spending and subscriptions. The goal is to provide a user-friendly interface to monitor financial activities visually.

**Challenges Faced**

One significant challenge encountered was related to the UseHttpsRedirection middleware in Program.cs, which caused the request body to lose the authorization header during testing on devices and emulators. After two days of debugging, the issue was identified, and the middleware was temporarily disabled, leading to successful testing. However, re-enabling it later worked fine.

**Future Plans**

Implement notifications to remind users of upcoming subscription due dates.
Conduct user testing and refine features based on feedback.
