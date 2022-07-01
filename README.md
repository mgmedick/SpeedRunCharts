Getting stated:

1. Clone the repo using your prefered method.
   - I use Visual Studio Code (vsc), so launch.json is already in the repo for debugging.
2. There is no hosted test database so you’ll have to set one up locally.
3. Make sure you have MySQL v8.0.29+ installed.
4. Add the following lines to the end of your mysqld.cnf (/etc/mysql/mysql.conf.d/mysqld.cnf).
   - lower_case_table_names = 1
   - optimizer_switch=block_nested_loop=off
   - group_concat_max_len = 1000000
4. Follow these instructions [how to import/export databases in mysql](https://www.digitalocean.com/community/tutorials/how-to-import-and-export-databases-in-mysql-or-mariadb) (step 2) to import the MySQL dump file.
   - Be sure to name the database "speedrunapp".
5. Change the connection string in appsettings.json to “”.

Happy debugging, feel free message me if you have any questions. 

Stack:

Vue.js
C#
MySQL
Vanilla JavaScript

Feature Requests::

The general focus of site is presenting the data in different ways than on speedrun.com

speedrun.com
Speedrunning leaderboards, resources, forums, and more!
speedrun.com
. With that in mind I’d like to stick to these guidelines when implementing new features.

Don’t implement features that modify the speedrun data.
Think of Speedrun.com as the data publisher. All moderation, run validation, game mgmt, etc happens there. 
Don’t implement features that requires moderators. 
I’d like this site to be entirely automated.
Don’t implement features that post to speedrun.com.
I don’t want to be responsible for screwed up data getting into their system and get banned from API.

Best Practices:

Avoid calling an API from the site.
Ideally the site should only read from it’s database. Any data from an API can be added to the import process and populate the db.
Don’t use bloated vue libraries.
There’s lots of bloated vue wrapper libraries out there. It’s often lighter weight to make your own component for your exact need (ex. your own multi-select wrapper using select 2 instead of vue-select library).
