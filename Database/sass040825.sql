-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 08, 2025 at 10:37 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `sass`
--

-- --------------------------------------------------------

--
-- Table structure for table `appointmentlogs`
--

CREATE TABLE `appointmentlogs` (
  `Id` int(11) NOT NULL,
  `AppointmentId` int(11) NOT NULL,
  `Action` varchar(20) NOT NULL,
  `Timestamp` datetime NOT NULL DEFAULT current_timestamp(),
  `ChangedByUserId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `appointmentlogs`
--

INSERT INTO `appointmentlogs` (`Id`, `AppointmentId`, `Action`, `Timestamp`, `ChangedByUserId`) VALUES
(1, 1, 'Created', '2025-04-04 12:00:00', 1);

-- --------------------------------------------------------

--
-- Table structure for table `appointments`
--

CREATE TABLE `appointments` (
  `Id` int(11) NOT NULL,
  `AssignedTo` int(11) NOT NULL,
  `Title` varchar(100) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `AppointmentDate` datetime NOT NULL,
  `StartTime` time NOT NULL,
  `EndTime` time NOT NULL,
  `Status` enum('pending','confirmed','cancelled','completed') NOT NULL,
  `Remarks` varchar(255) DEFAULT NULL,
  `DateCreated` datetime NOT NULL DEFAULT current_timestamp(),
  `DateModified` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `appointments`
--

INSERT INTO `appointments` (`Id`, `AssignedTo`, `Title`, `Name`, `AppointmentDate`, `StartTime`, `EndTime`, `Status`, `Remarks`, `DateCreated`, `DateModified`) VALUES
(1, 1, 'Dentist Appointment', 'Kiawa Cutie', '2025-04-10 00:00:00', '09:00:00', '10:00:00', 'pending', 'Bring medical records', '2025-04-04 14:35:24', '2025-04-04 14:35:24');

-- --------------------------------------------------------

--
-- Table structure for table `reminders`
--

CREATE TABLE `reminders` (
  `Id` int(11) NOT NULL,
  `AppointmentId` int(11) NOT NULL,
  `Type` varchar(20) NOT NULL,
  `Date` datetime NOT NULL,
  `Status` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `reminders`
--

INSERT INTO `reminders` (`Id`, `AppointmentId`, `Type`, `Date`, `Status`) VALUES
(1, 1, 'Email', '2025-04-05 09:00:00', 'Pending');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `Id` int(11) NOT NULL,
  `FirstName` varchar(100) NOT NULL,
  `LastName` varchar(100) NOT NULL,
  `Username` varchar(100) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Phone` varchar(20) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Role` enum('admin','user','pending') NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`Id`, `FirstName`, `LastName`, `Username`, `Email`, `Phone`, `PasswordHash`, `Role`, `IsActive`) VALUES
(1, 'Adrian Paul', 'Leano', 'admin', 'nrbsl.adrianleano@gmail.com', '(+63) 9277177738', '$2a$11$yQnf358YI.Mg148jpTKiGeaXrXlBt7a0beSCJremn7eTMXF2V3gxy', 'admin', 1),
(3, 'Kiawa', 'Cutie', 'user', 'user@gmail.com', '09123456789', '$2a$11$LP0m6JZlqkP81FCdqPYfOOWxiGBB9wDuRgQHxsJc3hkpL9f6aMjQC', 'user', 1),
(4, 'new', 'new', 'useruser', 'user1@gmail.com', '09123456789', '$2a$11$Dy53JBjDtmcaj6k2aDZ8u.bp.ZUORA81AzTTL.ntv1lFYI4JzIujO', 'user', 1),
(7, 'new', 'new', 'new', 'new@gmail.com', '09123456789', '$2a$11$zbGMryuvMATEuCeHTrXvLOSAOzbPd4/FnSi5CN90dC3FkHX4wgiNO', 'user', 1);

-- --------------------------------------------------------

--
-- Table structure for table `user_two_factor`
--

CREATE TABLE `user_two_factor` (
  `Id` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `SecretKey` varchar(255) NOT NULL,
  `IsEnabled` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `user_two_factor`
--

INSERT INTO `user_two_factor` (`Id`, `UserId`, `SecretKey`, `IsEnabled`) VALUES
(10, 1, 'S7L6RYXYOTGEBKRRZ6HPFQ2O7HYDPZSR', 1),
(12, 7, 'MRROBKR5MKVU33J3TW5CFFYMONLULU2I', 1);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `appointmentlogs`
--
ALTER TABLE `appointmentlogs`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `AppointmentId` (`AppointmentId`),
  ADD KEY `ChangedByUser` (`ChangedByUserId`);

--
-- Indexes for table `appointments`
--
ALTER TABLE `appointments`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `UserId` (`AssignedTo`);

--
-- Indexes for table `reminders`
--
ALTER TABLE `reminders`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `AppointmentId` (`AppointmentId`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Username` (`Username`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- Indexes for table `user_two_factor`
--
ALTER TABLE `user_two_factor`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `UserId` (`UserId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `appointmentlogs`
--
ALTER TABLE `appointmentlogs`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `appointments`
--
ALTER TABLE `appointments`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `reminders`
--
ALTER TABLE `reminders`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `user_two_factor`
--
ALTER TABLE `user_two_factor`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `appointmentlogs`
--
ALTER TABLE `appointmentlogs`
  ADD CONSTRAINT `appointmentlogs_ibfk_1` FOREIGN KEY (`AppointmentId`) REFERENCES `appointments` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `appointmentlogs_ibfk_2` FOREIGN KEY (`ChangedByUserId`) REFERENCES `users` (`Id`);

--
-- Constraints for table `appointments`
--
ALTER TABLE `appointments`
  ADD CONSTRAINT `appointments_ibfk_1` FOREIGN KEY (`AssignedTo`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `reminders`
--
ALTER TABLE `reminders`
  ADD CONSTRAINT `reminders_ibfk_1` FOREIGN KEY (`AppointmentId`) REFERENCES `appointments` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `user_two_factor`
--
ALTER TABLE `user_two_factor`
  ADD CONSTRAINT `user_two_factor_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
