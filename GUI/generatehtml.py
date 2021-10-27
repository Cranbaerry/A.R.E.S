import os, json
def makehtml(aci):
    avatars = json.loads(aci)
    html = """
<!DOCTYPE html>
<html>
   <head>
   <meta charset="UTF-8">
   <style>
        html{
    background-color: black;
}

div{
    display: inline-table;
    position: relative;
    text-align: center;
    padding: 5px;
    margin-inline-start: auto;


  }

.avatar-name{
  left: 15%;
  color: whitesmoke;

  opacity: 100%;
  text-align: center;
  font-size: 15px;
  font: bold;
  font-family: 'Open Sans', sans-serif;
  font-weight:bold;
  position: absolute;
  top: 5px;
  left: 10px;
  	text-shadow: -1px 1px 0 #000,
				  1px 1px 0 #000,
				 1px -1px 0 #000,
				-1px -1px 0 #000;


}
.avatar-id
{
  left: 15%;
  color: whitesmoke;

  opacity: 100%;
  text-align: center;
  font-size: 10px;
  font: bold;
  font-family: 'Open Sans', sans-serif;
  font-weight:bold;
  position: absolute;
  bottom: 15px;
  right: center;
  	text-shadow: -1px 1px 0 #000,
				  1px 1px 0 #000,
				 1px -1px 0 #000,
				-1px -1px 0 #000;


}
.avatar-releasestatus{
  right: 15%;
  font-size: 15px;
  position: absolute;
  top: -4px;
  right: -4px;


}
a:link {
  color: whitesmoke;
  text-decoration: none;

}

/* visited link */
a:visited {
  color: whitesmoke;
}
.avatar-download-link{

  right: 15%;
  color: whitesmoke;
  opacity: 100%;
  text-align: right;
  font-size: 10px;
  font: bold;
  font-family: 'Arial Narrow Bold', sans-serif;
  font-weight:bold;

  position: absolute;
  bottom: -4px;
  right: 10px;
}
.title-header{
  text-align: center;
  font-size: 50px;
  margin-bottom: 25px;
  color: whitesmoke;
  font-family: 'Open Sans', sans-serif;
  font-weight: bold;

}

html {
    overflow-y: scroll;
  }
.avatar-image{
  border-radius: 5px;
}

.hotbar-div{
  display: inline-table;
}
.hotbar-div2{

  display: inline-table;

}

.hotbar-search{
  display: inline-table;
}

.search-button{
  border: none;
  background-color: none;
  background: none;
  color:rgba(74,252,44,255);
  font-family: 'Open Sans', sans-serif;
  font-weight: bold;
  font-size: 16px;

}
.search-button:hover{
  cursor:pointer;
}
input{
  width: 100px;
  border: none;
  background-color: whitesmoke;
}

        html{
    background-color: black;
}
p{
    color: whitesmoke;
    font-family: 'Open Sans', sans-serif;
    font-size:initial;
    padding: 0;
    margin: 8.5px;



}
html {
    overflow-y: scroll;
  }
.title-header .getmainpage{
    text-align: center;
    font-size: 50px;
    margin-bottom: 25px;
    color: whitesmoke;
    font-family: 'Open Sans', sans-serif;
    font-weight: bold;


}
a:visited{
    text-decoration: none;

}
a:link{
    text-decoration: none;

}

.specialcolumn{
    display: inline-block;
    margin-right: 15px;
}


.centereddiv{
    margin-left: 25%;
}
.backbutton{
    border: none;
    background: none;
    color:rgba(74,252,44,255);
    font-family: 'Open Sans', sans-serif;
    font-weight: bold;
    font-size: 16px;

}
.downloadlink{
    font-size: initial;
    border: none;
    background: none;
    color:rgba(74,252,44,255);
    font-family: 'Open Sans', sans-serif;
    cursor:pointer;



}

.backbutton:hover{

    border: none;
    background: none;
    color:rgba(74,252,44,255);
    font-family: 'Open Sans', sans-serif;
    font-weight: bold;
    cursor:pointer;

}
    </style>
      <title class="maintitle">A.R.E.S Ripper</title>
      <link rel="preconnect" href="https://fonts.gstatic.com">
      <link href="https://fonts.googleapis.com/css2?family=Open+Sans:ital,wght@0,400;0,800;1,800&display=swap" rel="stylesheet">
   </head>
   <body>
   <a href="pp" class='title-header' class='getmainpage'><h1 class='getmainpage'>A.R.E.S Ripper<h1></a>


   </section>

"""
    for x in avatars:
        if x[9] == "private":
          html += f"""<div id = 'avatar-container' loading='lazy' ><img class='avatar-image' loading='lazy' src='{x[7]}'width='300' height='230'><p class='avatar-releasestatus' loading='lazy' >🔴</p><a  loading='lazy' class='avatar-name' href='{x[6]}' >{x[2]}</a><a  loading='lazy' class='avatar-id' >{x[1]}</a></div>"""
        else:
          html += f"""<div id = 'avatar-container' loading='lazy' ><img class='avatar-image' loading='lazy' src='{x[7]}'width='300' height='230'><p class='avatar-releasestatus' loading='lazy' >🟢</p><a  loading='lazy' class='avatar-name' href='{x[6]}' >{x[2]}</a><a  loading='lazy' class='avatar-id' >{x[1]}</a></div>"""


    html += """\n      </body>
</html>"""
    with open("avatars.html", "w+", encoding="utf-8") as kl:
            kl.write(html)
    os.system("avatars.html")