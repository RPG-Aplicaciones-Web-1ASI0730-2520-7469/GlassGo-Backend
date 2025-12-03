-- Migration: Create payment_methods table
-- Date: 2025-12-03
-- Description: Creates the payment_methods table for storing user payment methods

CREATE TABLE IF NOT EXISTS `payment_methods` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `Type` varchar(50) DEFAULT NULL,
  `Bank` varchar(100) DEFAULT NULL,
  `Account` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_payment_methods_UserId` (`UserId`),
  CONSTRAINT `FK_payment_methods_Users_UserId` 
    FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
