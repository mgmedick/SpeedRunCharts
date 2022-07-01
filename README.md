Getting stated:

1. Clone the repo using your prefered method.
   - Visual Studio Code and Visual Studio should work out of the box.
2. There is no hosted test database so you’ll have to set one up locally (steps 3-5).
3. Make sure you have MySQL v8.0.29 or later version installed, instructions below.
   - [how to install mysql linux](https://www.digitalocean.com/community/tutorial_collections/how-to-install-mysql) (choose your distro).
   - [how to install mysql windows](https://www.lifewire.com/how-to-install-mysql-windows-10-4584021)
5. Find your mysqld.cnf (linux) or default.ini (windows). If you run into any issues finding it just google where to find the file.
   - linux: mysqld.cnf is normally located "/etc/mysql/mysql.conf.d/".
   - windows: default.ini is normally located "C:\ProgramData\MySQL\MySQL Server 8.0\" (hidden folder).
7. Add the following lines to the end of your mysqld.cnf (or default.ini if windows).
   - lower_case_table_names = 1
   - optimizer_switch=block_nested_loop=off
   - group_concat_max_len = 1000000
8. Follow step 2 of these instructions [how to import/export databases in mysql](https://www.digitalocean.com/community/tutorials/how-to-import-and-export-databases-in-mysql-or-mariadb) to import the MySQL dump file.
   - Be sure to name the database "speedrunapp".
9. Fill in the "username" and "password" fields in the connection string (user is normally "root").

Happy debugging, feel free message me if you have any questions. 

Feature Request Guidelines:

The general focus of site is data analysis and presenting the data in different ways than on speedrun.com. With that in mind I’d like to stick to the guidelines below when requesting new features.

- Don’t implement features that modify the speedrun data.
  - Think of Speedrun.com as the data publisher. All moderation, run validation, game mgmt, etc happens there. 
- Don’t implement features that requires moderators. 
  - I’d like this site to be entirely automated.
- Don’t implement features that post to speedrun.com.
  - I don’t want to be responsible for screwed up data getting into their system and get banned from API.

Best Practices:

Avoid calling an API from the site.
Ideally the site should only read from it’s database. Any data from an API can be added to the import process and populate the db.
Don’t use bloated vue libraries.
There’s lots of bloated vue wrapper libraries out there. It’s often lighter weight to make your own component for your exact need (ex. your own multi-select wrapper using select 2 instead of vue-select library).


Stack:

Vue.js
C#
MySQL
Vanilla JavaScript

