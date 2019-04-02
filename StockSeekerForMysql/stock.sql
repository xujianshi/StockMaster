/*
 Navicat Premium Data Transfer

 Source Server         : 127.0.0.1
 Source Server Type    : MySQL
 Source Server Version : 50725
 Source Host           : 127.0.0.1:3306
 Source Schema         : stock

 Target Server Type    : MySQL
 Target Server Version : 50725
 File Encoding         : 65001

 Date: 15/03/2019 09:09:29
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for chengyu
-- ----------------------------
DROP TABLE IF EXISTS `chengyu`;
CREATE TABLE `chengyu`  (
  `ID` int(11) NOT NULL,
  `name` varchar(40) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `spell` varchar(80) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `content` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `derivation` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `samples` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for gov_stats_hyflbz
-- ----------------------------
DROP TABLE IF EXISTS `gov_stats_hyflbz`;
CREATE TABLE `gov_stats_hyflbz`  (
  `id` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '主键',
  `parentId` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `name` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `fullName` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `title` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '行业分类标准' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for gov_stats_tjypflml
-- ----------------------------
DROP TABLE IF EXISTS `gov_stats_tjypflml`;
CREATE TABLE `gov_stats_tjypflml`  (
  `id` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `parentId` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `name` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `link` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `JiBie` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for gov_stats_tjyqhdmhcxhfdm
-- ----------------------------
DROP TABLE IF EXISTS `gov_stats_tjyqhdmhcxhfdm`;
CREATE TABLE `gov_stats_tjyqhdmhcxhfdm`  (
  `Code` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Sheng` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Shi` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Xian` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Xiang` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Cun` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `JiBie` int(11) NULL DEFAULT NULL,
  `Url` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Cqlx` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Code`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '统计用区划和城乡划分代码http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2015/index.html' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for gov_stats_xzqhdm
-- ----------------------------
DROP TABLE IF EXISTS `gov_stats_xzqhdm`;
CREATE TABLE `gov_stats_xzqhdm`  (
  `ID` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ParentId` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Name` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FullName` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for stock
-- ----------------------------
DROP TABLE IF EXISTS `stock`;
CREATE TABLE `stock`  (
  `id` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `label` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ClosePrice` decimal(18, 2) NULL DEFAULT NULL,
  `OpenPrice` decimal(18, 2) NULL DEFAULT NULL,
  `PreClose` decimal(18, 2) NULL DEFAULT NULL,
  `MaxPrice` decimal(18, 2) NULL DEFAULT NULL,
  `MinPrice` decimal(18, 2) NULL DEFAULT NULL,
  `ZhenFu` decimal(18, 2) NULL DEFAULT NULL,
  `ZhangFu` decimal(18, 2) NULL DEFAULT NULL,
  `ZhangDieMoney` decimal(18, 2) NULL DEFAULT NULL,
  `Volume` decimal(18, 2) NULL DEFAULT NULL,
  `Amount` decimal(18, 2) NULL DEFAULT NULL,
  `HuanShoulv` decimal(18, 2) NULL DEFAULT NULL,
  `LiuTongShiZhi` decimal(18, 2) NULL DEFAULT NULL,
  `ZongShiZhi` decimal(18, 2) NULL DEFAULT NULL,
  `ShiYingLV` decimal(18, 2) NULL DEFAULT NULL,
  `ShiJingLV` decimal(18, 2) NULL DEFAULT NULL,
  `LimitDown` decimal(18, 2) NULL DEFAULT NULL,
  `LimitUp` decimal(18, 2) NULL DEFAULT NULL,
  `CreateDay` date NULL DEFAULT NULL,
  `LastUpdateTime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for stockhistory
-- ----------------------------
DROP TABLE IF EXISTS `stockhistory`;
CREATE TABLE `stockhistory`  (
  `rq` date NOT NULL,
  `code` bigint(20) NOT NULL,
  `name` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `closeprice` double(16, 2) NULL DEFAULT NULL,
  `highprice` double(16, 2) NULL DEFAULT NULL,
  `lowprice` double(16, 2) NULL DEFAULT NULL,
  `openprice` double(16, 2) NULL DEFAULT NULL,
  `lastprice` double(16, 2) NULL DEFAULT NULL,
  `zhangdiee` double(16, 2) NULL DEFAULT NULL,
  `zhangfu` double(16, 2) NULL DEFAULT NULL,
  `huanshoulv` double(16, 2) NULL DEFAULT NULL,
  `chengjiaoliang` double(16, 2) NULL DEFAULT NULL,
  `chengjiaojine` double(16, 2) NULL DEFAULT NULL,
  `zongshizhi` double NULL DEFAULT NULL,
  `liutongshizhi` double NULL DEFAULT NULL,
  PRIMARY KEY (`rq`, `code`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for stocknews
-- ----------------------------
DROP TABLE IF EXISTS `stocknews`;
CREATE TABLE `stocknews`  (
  `id` bigint(20) NOT NULL,
  `code` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL DEFAULT NULL,
  `title` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `KeyWords` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ZhengWen` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Source` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for stockprice
-- ----------------------------
DROP TABLE IF EXISTS `stockprice`;
CREATE TABLE `stockprice`  (
  `code` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `rq` date NOT NULL,
  `OpenPrice` decimal(18, 2) NULL DEFAULT NULL,
  `ClosePrice` decimal(18, 2) NOT NULL,
  `HighPrice` decimal(18, 2) NULL DEFAULT NULL,
  `LowPrice` decimal(18, 2) NULL DEFAULT NULL,
  `ZhangFu` decimal(18, 2) NULL DEFAULT NULL,
  `ZhenFu` decimal(18, 2) NULL DEFAULT NULL,
  `HuanShou` decimal(18, 2) NULL DEFAULT NULL,
  `Volume` decimal(18, 2) NULL DEFAULT NULL,
  `Amount` decimal(18, 2) NULL DEFAULT NULL,
  `State` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`code`, `rq`) USING BTREE,
  INDEX `code`(`code`) USING BTREE,
  INDEX `rq`(`rq`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for stocktraderecord
-- ----------------------------
DROP TABLE IF EXISTS `stocktraderecord`;
CREATE TABLE `stocktraderecord`  (
  `Id` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `TradeTime` datetime(0) NULL DEFAULT NULL,
  `code` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `tradetype` int(255) NULL DEFAULT NULL,
  `price` double(255, 2) NULL DEFAULT NULL,
  `num` int(255) NULL DEFAULT NULL,
  `chengjiaoMoney` double(255, 2) NULL DEFAULT NULL,
  `totalmoney` double(255, 2) NULL DEFAULT NULL,
  `OtherMoney` double(255, 2) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
