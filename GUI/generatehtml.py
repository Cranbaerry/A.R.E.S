import os, json
def makehtml(aci):
    avatars = json.loads(aci)
    html = """<!DOCTYPE html>
<html>
<head>
<style>
figure {
margin:2px;
display:inline-block;
vertical-align:top;
border:solid gray;
text-align:center;
position:relative;
padding-bottom:2.8em;
}
figure img {
display:block;
border:solid white
}
figcaption{
background:blue;
position:absolute;
bottom:0;
left:0;
right:0;
padding:0.2em 2px;
min-height:2.4em;
opacity:1;
transition:0.5s;
color:white;
}
figcaption p {
margin:0;
}
#gridimg, #gridimg2 {
min-width:560px;
max-width:840px;
margin:auto;
}
#gridimg:first-line {
position:absolute;
color:green;
}
figure:nth-child(odd) figcaption {
color:white;
}
#gridimg2 figure {
padding:0;
}
#gridimg2 figure:hover figcaption {
opacity:0;
}
figcaptionpriv{
background:red;
position:absolute;
bottom:0;
left:0;
right:0;
padding:0.2em 2px;
min-height:2.4em;
opacity:1;
transition:0.5s;
color:white;
}
figcaptionpriv p {
margin:0;
}
#gridimg, #gridimg2 {
min-width:560px;
max-width:840px;
margin:auto;
}
#gridimg:first-line {
position:absolute;
color:green;
}
figure:nth-child(odd) figcaptionpriv {
color:white;
}
#gridimg2 figure {
padding:0;
}
#gridimg2 figure:hover figcaptionpriv {
opacity:0;
}
body {
text-align:center;
background-color:black;
}
</style>
</head>
<body>
<h1>AVATAR SEARCH</h1>
<div id="gridimg">\n"""
    for x in avatars:
        #input(x)
        if x[9] == "private":
            html += f"""<figure>
    <img src="{x[7]}" width="250" 
     height="250" />
    <figcaptionpriv>
      <p>{x[1]}</p>
    </figcaptionpriv>
    </figure>"""
        else:
            html += f"""<figure>
                <img src="{x[7]}" width="250" 
                 height="250" />
                <figcaption>
                  <p>{x[1]}</p>
                </figcaption>
              </figure>"""
    html += """\n</div>
</div>
</body>
</html>"""
    with open("avatars.html", "w+") as kl:
            kl.write(html)
    os.system("avatars.html")