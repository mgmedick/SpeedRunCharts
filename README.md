## About

An open source site that displays and analyzes data from [speedrun.com](https://www.speedrun.com).

## Getting stated:

1. Clone the repo using your preferred method.
   - Visual Studio Code and Visual Studio should work out of the box.
2. There is no hosted test database so you’ll have to set one up locally (steps 3-5).
3. Make sure you have MySQL v8.0.29 or later version installed, instructions below.
   - [how to install mysql linux](https://www.digitalocean.com/community/tutorial_collections/how-to-install-mysql) (choose your distro).
   - [how to install mysql windows](https://www.lifewire.com/how-to-install-mysql-windows-10-4584021)
5. Find your mysqld.cnf (linux) or default.ini (windows) file.
   - linux: mysqld.cnf is normally located "/etc/mysql/mysql.conf.d/".
   - windows: default.ini is normally located "C:\ProgramData\MySQL\MySQL Server 8.0\" (hidden folder).
7. Add the following lines to the end of your mysqld.cnf (or default.ini if windows).
   - lower_case_table_names = 1
   - optimizer_switch=block_nested_loop=off
   - group_concat_max_len = 1000000
8. Follow step 2 of these instructions [how to import/export databases in mysql](https://www.digitalocean.com/community/tutorials/how-to-import-and-export-databases-in-mysql-or-mariadb) and import the [speedrunapp_test_dump.sql](https://github.com/mgmedick/SpeedRunChartsDatabaseScripts/blob/master/MySQL/speedrunapp_test_dump.sql) MySQL dump file.
9. Edit the connection string in the "appsettings.json" file with your MySQL credentials, user (usually "root") and password.
10. Start debugging, email/message me if you have any questions.

## Import Process

The Games, Players and Runs are imported from [speedrun.com api](https://github.com/speedruncomorg/api). The site imports verified runs every 15 mins and updates game definitions nightly. Import process code can be found here [SpeedRunChartsImport](https://github.com/mgmedick/SpeedRunChartsImport).

## Database Scripts

The Database Schema script can be found here [SpeedRunAppImportJRE_DeployScript.sql](https://github.com/mgmedick/SpeedRunChartsDatabaseScripts/blob/master/JRE/SpeedRunAppImportJRE_DeployScript.sql).

## Stack:

- Vue.js
- .Net 6
- C#
- MySQL
- Vanilla JavaScript
- Bootstrap 5.3

