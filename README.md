## About

Speedruncharts.com is an open source site that imports and displays the data from Speedrun.com. The site focuses on different ways of viewing and analyzing the speedrun.com data (runs, games, users, moderators, etc). The idea is to implement features the speedrun community is looking for that aren't avaiable on speedrun.com. This site isn't meant as a replacement for speedrun.com, more a complimentary feature set to their data. If you have any feature requests I’d love to hear them, feel free to open an issue here on Github (check out feature requests guidelines first).

## Getting stated:

1. Clone the repo using your prefered method.
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
8. Follow step 2 of these instructions [how to import/export databases in mysql](https://www.digitalocean.com/community/tutorials/how-to-import-and-export-databases-in-mysql-or-mariadb) and import the "speedrunapp_tes_dump.sql" MySQL dump file.
   - Be sure to name the database "speedrunapp_test".
9. Edit the connection string in the "appsettings.json" file with your MySQL credentials, user (usually "root") and password.

Happy debugging, feel free message me if you have any questions. 

## Feature Request Guidelines:

The general focus of site is data analysis and presenting the data in different ways than on speedrun.com. With that in mind I’d like to stick to the guidelines below when requesting new features.

- Don’t implement features that modify the speedrun data.
  - Think of Speedrun.com as the data publisher. All moderation, run validation, game mgmt, etc happens there. This site just shows that data in different ways (ex. world records, speedrun videos with endless scroll, etc). 
- Don’t implement features that require moderators. 
  - I’d like this site to be entirely automated.
- Don’t implement features that post to speedrun.com.
  - I don’t want to mess up their data and potentially get banned from the API.

## Import Process

Currently the import code isn't open source, but I plan to share the API wrapper in the near future. The import runs every 15 mins and populates all the data the sites uses. So all API calls (speedrun.com api, twitch, youtube, etc.) are made from the import. Additionally the import only pulls verified runs, so the site only has verified data.

## Best Practices:

- Avoid calling an API from the site.
  - Generally API Calls should be made from the import. I'm happy to take on any import enhancements you need for a feature. Exceptions can be made for API's that make more sense to call from the site.
- Don’t use bloated libraries.
  - There’s lots of bloated vue libraries that are just wrappers of popular js libraries. It's lighter weight to make your own component (ex. your own vue mulitselect component using select2).
- Use vanilla javascript.
  - Be sure your js is cross browser compatible with latest mainstream browsers (chromium, firefox, safari).
  - No need for backwards compaitibility with old browsers (especially IE 11 and below).
- Put clientside logic in Vue.js.
  - I'd like to eventually switch to Vue Router and have the server just be ajax endpoints (no cshtml pages, all vue pages).
  - For now cshtml pages should mainly be a shell containing vue compoenents (just need the pages for MVC routing).

## Stack:

- Vue.js
- .Net Core MVC 3.1
- C#
- MySQL
- Vanilla JavaScript
- Bootstrap 4

## Contact Info:

Happy coding! If you have any questions you can message me here on GitHub or you can contact me at [speedruncharts@outlook.com](mailto:speedruncharts@outlook.com).
