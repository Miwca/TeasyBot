<img src="https://github.com/Miwca/TeasyBot/blob/main/Images/Teasy.png?raw=true" width="100" height="100" align="right" />

# Teasy Bot
Bot has been made and rebranded by **Miwca** and **Lat3xKitty**.

## Description
First developed for the Lovense Easter Dev Jam "Egg Hunt" challenge, Naughty Bunny Bot was a Discord bot that allowed users to do a mini server wide egg hunt - Now renamed and refactored to tease new product releases for Lovense. The bot will randomly place hints in the server and users can interact with the hint (via a button). The user with the most eggs at the end of the event wins.

## Setup
Initial Requirements:
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) 
- [MySQL Server](https://severalnines.com/blog/mysql-docker-building-container-image/)
- [Discord Bot](https://discord.com/developers/applications)
- [Lovense Developer Account](https://www.lovense.com/user/account/profile)

### Database Setup (MySQL)
> Assumes MySQL server is already installed and running  
> This script will create the database, user, and required tables  
> You can run this in MySQL CLI, Workbench, phpMyAdmin, DBeaver, etc.

```sql
-- Create database
CREATE DATABASE IF NOT EXISTS teasy
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

-- Create a dedicated user (optional but recommended)
CREATE USER IF NOT EXISTS 'teasy'@'%' IDENTIFIED BY 'Password1';
GRANT ALL PRIVILEGES ON teasy.* TO 'teasy'@'%';
FLUSH PRIVILEGES;

-- Select the DB
USE teasy;

-- Table: ApprovedChannels
CREATE TABLE IF NOT EXISTS `ApprovedChannels` (
  `Id` CHAR(36) NOT NULL DEFAULT (UUID()),
  `GuildId` VARCHAR(50) NOT NULL,
  `ChannelId` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_unicode_ci;

-- Table: Leaderboard
CREATE TABLE IF NOT EXISTS `Leaderboard` (
  `Id` CHAR(36) NOT NULL DEFAULT (UUID()),
  `UserId` VARCHAR(50) NOT NULL,
  `GuildId` VARCHAR(50) NOT NULL,
  `Score` INT NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_unicode_ci;
```

### Setup Bot
1. Clone the repository
2. Open inside of VS Code or Visual Studio (for best results)
3. Setup your appsettings.json
   - Replace the `Discord.Token` with your bot token
   - Replace the `Discord.Admins` with the list of user ids that are allowed to run admin commands
   - Replace the `Lovense.DeveloperToken` with your Lovense API token
   - Replace the `ConnectionStrings.Leaderboard` with your connection string to a MySQL Database.
4. Debug the bot and enjoy!

### Setup the bot for Docker
1. Clone the repository
2. Open terminal of choice
3. Navigate to ``/Src``
4. Run ``docker build -t teasybot -f TeasyBot.Service/Dockerfile .``
5. Run 
```
docker run -d --restart always --name teasybot --network bridge -e ConnectionStrings__Leaderboard="Server=127.0.0.1;Database=teasy;User=teasy;Password=Password1" -e Discord__Token="SuperSecretTokenhere" -e Lovense__DeveloperToken="SuperSecretLovenseTokenHere" teasybot
```
6. Enjoy!

## Setup (Discord side)
1. Invite the bot to your server
2. Use `/addchannel` to add a channel to the egg hunt - This is where the eggs will be placed
   - You can use `/listchannel` to see the list of channels that are currently enabled.
3. Use `/enable` in an announcement channel to start the egg hunt

## How to use and play with the bot?
- `/enable` will send out a Message with a button to join the event.
  - A Lovense QR code will be given for the user to connect their toy for enhanced play 😏
- Randomly a Egg will be let loose in a channel
  - User will be able to collect the egg via clicking a button on the egg and will be given a point. A pattern matching the egg will play as a nice reward.
  - But careful a dud egg may also be placed.
- `/leaderboard` will show the top 10 users
- `/profile` will show your current egg count
