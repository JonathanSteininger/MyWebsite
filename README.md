# MyWebsite

<h1>What the website is for</h1>
This website is meant to hold all the information an employer would want when looking into me.
such as:
<ul>
  <li>Information about me</li>
  <li>My projects</li>
  <li>My experience</li>
  <li>How to contace me</li>
  <li>general information about me</li>
</ul>

I want this to be a beacon to show what I can acomplish. I want it to impress anyone on me website.

<h1>What will the website have?</h1>

<h2>FrameWork</h2>
This website is built on the ASP.NET core framework, allowing me to create dynamic pages using c# in an MVC environment.

<h3>Identities</h3>
I will be using Identities to handle users, but I'll only have an admin account anyways.

<h3>Database</h3>
ASP.Net core allows me to easily use a database to store all the information on the webpage, and then dynamically load pages depending on the informmation iside the database.

<h3>Libraries</h3>
I will be using libraries such as bootstrap to make styling the pages way easier. 

<h2>Who is allowed to collaborate?</h2>
I am making this project for myself to show off my programming knowledge, and therefore will be making this website on my own.

I want to use this time to practice committing to github and show that I am able to use github effectivly.

Employers will probably check my github account and check how well I use it, how often I commit, and some of my big projects.

<h1>Why is this open source?</h1>

<p>
  I see no harm in letting it be open source.<br/>
  I can easily prove that I made this website, and display it as one of my projects, any vulnrabilities that come from this website being open source will just help it get more secure.<br/>
  This website wont be hosting any sensitive data, and if the database gets wiped by somehow doing an sql injection ill just have a backup anyways. :P <br/>
  this project is way more valuable if I let people that go to the webpage be able to view the source code. I also like having stuff be open source!
</p>


<h1>
  Want to view the program in action?
</h1>
<h2>Setup</h2>
<p>
  in order to actualy run my selution, youll need something that can run asp.net core, i recomment visual studio 2022 since that is the environment that I used.<br/>
  Then youll need to do the following:
  <ul>
    <li>
      Update the database in the nuGet package manager console by running: "Update-database"
    </li>
    <li>
      Manually add an admin account into the database.
      <ul>
        <li>
          make sure the mannual routing on the program.cs is removed to allow adding an account into the database.
        </li>
        <li>
          run the program and manually navigate to /Identity/Account/Login in the URL.
        </li>
         <li> 
           add an account, make sure to remember the password and email.
        </li>
         <li>
           now stop the application.
        </li>
         <li>
           open the SQL server object explorer through the View tab.
        </li>
        <li>
          browse into the database called "aspnet-myactualwebsite-{some numbers}"
        </li>
        <li>
          go into the tables and right click "AspNetRoles" and click on view data
        </li>
        <li> 
          enter an id of "1" the name of "Admin" and the normalizedName of "ADMIN" and update the table.
        </li>
        <li> 
          now right click the table called "AspNetUsers"  and click on view data
        </li>
        <li>
          copy the entire id field of the user you created.
        </li>
        <li> 
          now right click on the table called "AspNetUserRoles" and click on view data
        </li>
        <li>
          Paste the user id into the userID field. and put "1" in the RoleID field.
        </li>
        <li>
          You should now be able to run the app. log into the account, and have admin access to the webpage!
        </li>
      </ul>
    </li>
    <li>
      Now you need to make some Tag catagories, do this by heading into the TagCatagory page. and create these tags: "Language", "Platform", "FrameWork", "Other".
    </li>
    <li>
      now you can make some tags in th tags page. make as many as you want with the appropriate tagCatagory.
    </li>
    <li> 
      now create a project in the projects page. making it featured will make it display in the home page.
    </li>
    <li> 
      now add tags to those project in the "Tags to project" page. this is where you add tags to projects.
    </li>
    <li> 
      you can now view the project in the portfolio.<br/>
      Different sized projects will display slightly different project screens.<br/>
      you can filter the portfolio page with the tags you made. <br/>
      the portfolio page is using a flexbox, so having more projects will give a better result.
    </li>
    <li>
      now youll need to fill the rest of the database. 
      add some data to the StatBars page, and the FAQs page.
    </li>
  </ul>
</p>
