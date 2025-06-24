Habit Tracker API
專案名稱：Habit Tracker API
用途：提供習慣建立、打卡、報表分析等功能的後端服務
技術棧：ASP.NET Core Web API、SQL Server、Dapper、Swagger

功能簡介
習慣管理：CRUD 功能，建立與管理使用者習慣

打卡功能：每日完成習慣打卡，支持打卡記錄查詢

報表分析：提供週報、成長趨勢、行為風格分析等報表

提醒優先排序：根據使用者行為計算提醒優先級，提升使用黏著度

技術架構
ASP.NET Core 8 Web API

SQL Server 與 Stored Procedure 儲存與計算

Dapper 作為輕量 ORM

Swagger UI 方便 API 測試與文件

分層架構（Controller、Service、Repository、DTO）

API 文件與測試
啟動專案後，打開 Swagger UI 頁面：
https://localhost:{port}/swagger/index.html

環境設定與啟動
安裝 .NET 8 SDK

安裝 SQL Server 並恢復資料庫備份或建立資料表

更新 appsettings.json 中的連線字串

使用 Visual Studio 或命令列啟動 API 專案

透過 Swagger 或 Postman 測試 API

重要 API 範例
建立習慣：POST /api/habit

打卡習慣：POST /api/habit/{id}/track

取得週報：GET /api/reports/weekly?userId=1&start=2025-06-01&end=2025-06-07

取得成長趨勢：GET /api/reports/growth-trend?userId=1&start=2025-05-01&end=2025-05-31

行為風格分析：GET /api/analysis/style?userId=1&start=2025-06-01&end=2025-06-30

未來擴充方向
推播通知整合（Firebase Cloud Messaging）

使用者偏好提醒設定

打卡異常偵測與提醒

社群互動與共習功能

聯絡方式
作者：cafelee
Email：cafe307187749@gmail.com
