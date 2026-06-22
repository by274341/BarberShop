/*
 Navicat MySQL Dump SQL

 Source Server         : Desgin
 Source Server Type    : MySQL
 Source Server Version : 80040 (8.0.40)
 Source Host           : localhost:3306
 Source Schema         : salon_booking

 Target Server Type    : MySQL
 Target Server Version : 80040 (8.0.40)
 File Encoding         : 65001

 Date: 22/06/2026 15:29:35
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for appointments
-- ----------------------------
DROP TABLE IF EXISTS `appointments`;
CREATE TABLE `appointments`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `CustomerId` int NOT NULL,
  `StylistId` int NULL DEFAULT NULL,
  `TotalPrice` decimal(65, 30) NOT NULL,
  `AppointmentDate` datetime(6) NOT NULL,
  `TimeSlot` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Status` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Note` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `StylistId1` int NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_Appointments_CustomerId`(`CustomerId` ASC) USING BTREE,
  INDEX `IX_Appointments_StylistId`(`StylistId` ASC) USING BTREE,
  INDEX `IX_Appointments_StylistId1`(`StylistId1` ASC) USING BTREE,
  CONSTRAINT `FK_Appointments_Stylists_StylistId1` FOREIGN KEY (`StylistId1`) REFERENCES `stylists` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_Appointments_Users_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `users` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_Appointments_Users_StylistId` FOREIGN KEY (`StylistId`) REFERENCES `users` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 11 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appointments
-- ----------------------------
INSERT INTO `appointments` VALUES (1, 2, 1, 10.000000000000000000000000000000, '2026-06-16 16:00:00.000000', '09:00-09:30', 'Cancelled', '2026-06-17 14:13:29.455119', NULL, NULL);
INSERT INTO `appointments` VALUES (2, 2, NULL, 10.000000000000000000000000000000, '2026-06-17 16:00:00.000000', '10:00-10:30', 'Cancelled', '2026-06-17 14:15:10.200267', NULL, NULL);
INSERT INTO `appointments` VALUES (3, 2, 1, 5.000000000000000000000000000000, '2026-06-16 16:00:00.000000', '10:00-10:30', 'Confirmed', '2026-06-17 14:20:21.278877', NULL, NULL);
INSERT INTO `appointments` VALUES (4, 2, 2, 30.000000000000000000000000000000, '2026-06-16 16:00:00.000000', '15:30-16:00', 'Confirmed', '2026-06-17 14:22:52.515974', NULL, NULL);
INSERT INTO `appointments` VALUES (5, 2, 1, 5.000000000000000000000000000000, '2026-06-21 16:00:00.000000', '09:30-10:00', 'Completed', '2026-06-22 15:08:49.463827', NULL, NULL);
INSERT INTO `appointments` VALUES (6, 2, 3, 5.000000000000000000000000000000, '2026-06-21 16:00:00.000000', '10:00-10:30', 'Confirmed', '2026-06-22 15:21:36.111703', NULL, NULL);
INSERT INTO `appointments` VALUES (7, 2, 3, 30.000000000000000000000000000000, '2026-06-22 16:00:00.000000', '09:30-10:00', 'Confirmed', '2026-06-22 15:23:06.939911', NULL, NULL);
INSERT INTO `appointments` VALUES (8, 2, 3, 5.000000000000000000000000000000, '2026-06-23 16:00:00.000000', '09:30-10:00', 'Confirmed', '2026-06-22 15:23:48.888445', NULL, NULL);
INSERT INTO `appointments` VALUES (9, 2, 3, 5.000000000000000000000000000000, '2026-06-21 16:00:00.000000', '10:00-10:30', 'Confirmed', '2026-06-22 15:24:41.248110', NULL, NULL);
INSERT INTO `appointments` VALUES (10, 2, 3, 5.000000000000000000000000000000, '2026-06-21 16:00:00.000000', '15:30-16:00', 'Confirmed', '2026-06-22 15:25:53.843377', NULL, NULL);

-- ----------------------------
-- Table structure for appointmentservices
-- ----------------------------
DROP TABLE IF EXISTS `appointmentservices`;
CREATE TABLE `appointmentservices`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AppointmentId` int NOT NULL,
  `ServiceId` int NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_AppointmentServices_AppointmentId`(`AppointmentId` ASC) USING BTREE,
  INDEX `IX_AppointmentServices_ServiceId`(`ServiceId` ASC) USING BTREE,
  CONSTRAINT `FK_AppointmentServices_Appointments_AppointmentId` FOREIGN KEY (`AppointmentId`) REFERENCES `appointments` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_AppointmentServices_Services_ServiceId` FOREIGN KEY (`ServiceId`) REFERENCES `services` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 11 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appointmentservices
-- ----------------------------
INSERT INTO `appointmentservices` VALUES (1, 1, 1);
INSERT INTO `appointmentservices` VALUES (2, 2, 1);
INSERT INTO `appointmentservices` VALUES (3, 3, 1);
INSERT INTO `appointmentservices` VALUES (4, 4, 2);
INSERT INTO `appointmentservices` VALUES (5, 5, 1);
INSERT INTO `appointmentservices` VALUES (6, 6, 1);
INSERT INTO `appointmentservices` VALUES (7, 7, 2);
INSERT INTO `appointmentservices` VALUES (8, 8, 1);
INSERT INTO `appointmentservices` VALUES (9, 9, 1);
INSERT INTO `appointmentservices` VALUES (10, 10, 1);

-- ----------------------------
-- Table structure for coupons
-- ----------------------------
DROP TABLE IF EXISTS `coupons`;
CREATE TABLE `coupons`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FaceValue` decimal(65, 30) NOT NULL,
  `MinAmount` decimal(65, 30) NOT NULL,
  `ValidFrom` datetime(6) NOT NULL,
  `ValidTo` datetime(6) NOT NULL,
  `TotalCount` int NOT NULL,
  `IssuedCount` int NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of coupons
-- ----------------------------
INSERT INTO `coupons` VALUES (1, '测试优惠券1', 5.000000000000000000000000000000, 10.000000000000000000000000000000, '2026-06-16 16:00:00.000000', '2026-06-26 16:00:00.000000', 100, 0);

-- ----------------------------
-- Table structure for favorites
-- ----------------------------
DROP TABLE IF EXISTS `favorites`;
CREATE TABLE `favorites`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `CustomerId` int NOT NULL,
  `StylistId` int NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_Favorites_CustomerId`(`CustomerId` ASC) USING BTREE,
  INDEX `IX_Favorites_StylistId`(`StylistId` ASC) USING BTREE,
  CONSTRAINT `FK_Favorites_Stylists_StylistId` FOREIGN KEY (`StylistId`) REFERENCES `stylists` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_Favorites_Users_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `users` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of favorites
-- ----------------------------
INSERT INTO `favorites` VALUES (1, 2, 1);
INSERT INTO `favorites` VALUES (3, 2, 2);

-- ----------------------------
-- Table structure for promotions
-- ----------------------------
DROP TABLE IF EXISTS `promotions`;
CREATE TABLE `promotions`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ServiceId` int NOT NULL,
  `DiscountRate` decimal(65, 30) NOT NULL,
  `StartTime` datetime(6) NOT NULL,
  `EndTime` datetime(6) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_Promotions_ServiceId`(`ServiceId` ASC) USING BTREE,
  CONSTRAINT `FK_Promotions_Services_ServiceId` FOREIGN KEY (`ServiceId`) REFERENCES `services` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of promotions
-- ----------------------------
INSERT INTO `promotions` VALUES (1, '测试活动1', 1, 0.500000000000000000000000000000, '2026-06-16 16:00:00.000000', '2026-06-26 16:00:00.000000', 1);

-- ----------------------------
-- Table structure for reviews
-- ----------------------------
DROP TABLE IF EXISTS `reviews`;
CREATE TABLE `reviews`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AppointmentId` int NOT NULL,
  `CustomerId` int NOT NULL,
  `StylistId` int NOT NULL,
  `Rating` int NOT NULL,
  `Content` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_Reviews_AppointmentId`(`AppointmentId` ASC) USING BTREE,
  INDEX `IX_Reviews_CustomerId`(`CustomerId` ASC) USING BTREE,
  INDEX `IX_Reviews_StylistId`(`StylistId` ASC) USING BTREE,
  CONSTRAINT `FK_Reviews_Appointments_AppointmentId` FOREIGN KEY (`AppointmentId`) REFERENCES `appointments` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_Reviews_Stylists_StylistId` FOREIGN KEY (`StylistId`) REFERENCES `stylists` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_Reviews_Users_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `users` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of reviews
-- ----------------------------
INSERT INTO `reviews` VALUES (1, 1, 2, 1, 5, '111', '2026-06-17 14:17:19.854622');

-- ----------------------------
-- Table structure for services
-- ----------------------------
DROP TABLE IF EXISTS `services`;
CREATE TABLE `services`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Category` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Price` decimal(65, 30) NOT NULL,
  `Duration` int NOT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Image` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `IsDiscountable` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of services
-- ----------------------------
INSERT INTO `services` VALUES (1, '测试项目1', '1', 10.000000000000000000000000000000, 30, '111', '', 1, 1);
INSERT INTO `services` VALUES (2, '测试项目2', '2', 30.000000000000000000000000000000, 60, '222', '', 1, 1);

-- ----------------------------
-- Table structure for stylists
-- ----------------------------
DROP TABLE IF EXISTS `stylists`;
CREATE TABLE `stylists`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `Level` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `YearsOfExperience` int NOT NULL,
  `Specialty` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Status` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_Stylists_UserId`(`UserId` ASC) USING BTREE,
  CONSTRAINT `FK_Stylists_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of stylists
-- ----------------------------
INSERT INTO `stylists` VALUES (1, 3, '初级', 1, '111', '111', 'Active');
INSERT INTO `stylists` VALUES (2, 4, '中级', 2, '222', '222', 'Active');

-- ----------------------------
-- Table structure for stylistschedules
-- ----------------------------
DROP TABLE IF EXISTS `stylistschedules`;
CREATE TABLE `stylistschedules`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StylistId` int NOT NULL,
  `DayOfWeek` int NOT NULL,
  `StartTime` time(6) NOT NULL,
  `EndTime` time(6) NOT NULL,
  `IsOff` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_StylistSchedules_StylistId`(`StylistId` ASC) USING BTREE,
  CONSTRAINT `FK_StylistSchedules_Stylists_StylistId` FOREIGN KEY (`StylistId`) REFERENCES `stylists` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of stylistschedules
-- ----------------------------

-- ----------------------------
-- Table structure for stylisttags
-- ----------------------------
DROP TABLE IF EXISTS `stylisttags`;
CREATE TABLE `stylisttags`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StylistId` int NOT NULL,
  `Tag` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_StylistTags_StylistId`(`StylistId` ASC) USING BTREE,
  CONSTRAINT `FK_StylistTags_Stylists_StylistId` FOREIGN KEY (`StylistId`) REFERENCES `stylists` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of stylisttags
-- ----------------------------

-- ----------------------------
-- Table structure for usercoupons
-- ----------------------------
DROP TABLE IF EXISTS `usercoupons`;
CREATE TABLE `usercoupons`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `CouponId` int NOT NULL,
  `IsUsed` tinyint(1) NOT NULL,
  `UsedAt` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_UserCoupons_CouponId`(`CouponId` ASC) USING BTREE,
  INDEX `IX_UserCoupons_UserId`(`UserId` ASC) USING BTREE,
  CONSTRAINT `FK_UserCoupons_Coupons_CouponId` FOREIGN KEY (`CouponId`) REFERENCES `coupons` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_UserCoupons_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of usercoupons
-- ----------------------------

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Role` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Nickname` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Avatar` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Phone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of users
-- ----------------------------
INSERT INTO `users` VALUES (1, 'admin', '$2a$11$EkXJykywnJxliQHarvb3g.ErKCX9GOxMmjCISR/sSeqMQlGB25Ay6', 'Manager', '店长', NULL, '13800000000', '2026-06-17 13:50:32.090250');
INSERT INTO `users` VALUES (2, '123', '$2a$11$V.NLkAqetU3Gh07/51NIR.2Ts3KrYN1w2b.60QbJXG3Lki92G1t8q', 'Customer', '测试1', NULL, '11111111111', '2026-06-17 13:52:43.768406');
INSERT INTO `users` VALUES (3, 'stylist_639173022719725187', '$2a$11$TBIaMMqiQRZT.9uQ.chiMedVwrfu.keICRlMn1NXl8DZ0uxTi1DDm', 'Stylist', '测试发型师1', NULL, '', '2026-06-17 14:11:12.157791');
INSERT INTO `users` VALUES (4, 'stylist_639173028982636729', '$2a$11$C3urr92uhbI7Rxz0qytyfu/wUJRS6v51fx8ZZDcOCnP3K2xYZBH9m', 'Stylist', '测试发型师2', NULL, '', '2026-06-17 14:21:38.387397');

SET FOREIGN_KEY_CHECKS = 1;
