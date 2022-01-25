import dateparser, html

inlist = '<>'
idd = 0


def genhtml(avis):
    global idd
    strr = """<!DOCTYPE html>
<html lang="en">

    <head>
        <meta charset="utf-8" />
        <link rel="icon" href="" />
        <meta name="viewport" content="width=device-width,initial-scale=1" />

        <title>A.R.E.S Results</title>
        <link rel="stylesheet" href="cssstuff/intlTelInput.css">
        <link rel="stylesheet" href="cssstuff/bootstrap.min.css" />
        <link rel="stylesheet" href="cssstuff/site_2.css" />
        <style>
            .margined {
                max-width: 1500px;
            }
        </style>


    </head>

<body>
    <button onclick="topFunction()" id="topbtn" title="Go to top">Top</button>
    <script>
        //Get the button:
        mybutton = document.getElementById("topbtn");

        // When the user scrolls down 20px from the top of the document, show the button
        window.onscroll = function() {scrollFunction()};

        function scrollFunction() {
        if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
            mybutton.style.display = "block";
        } else {
            mybutton.style.display = "none";
        }
        }

        // When the user clicks on the button, scroll to the top of the document
        function topFunction() {
            var a = document.documentElement.scrollTop;
            var b = document.documentElement.scrollHeight - document.documentElement.clientHeight;
            var c = a / b;
            var sspost = c
            localStorage && (localStorage.scrollheight = sspost);
            document.body.scrollTop = 0; // For Safari
            document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
        }
        function backscroll() {
            if (localStorage && 'scrollheight' in localStorage) {
                var sspost = localStorage.scrollheight;
                }
            document.body.scrollTop = sspost; // For Safari
            document.documentElement.scrollTop = sspost; // For Chrome, Firefox, IE and Opera
        }

    </script>

    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-dark box-shadow mb-3">
            <div class="container">
                <a class="navbar-brand" href="/">A.R.E.S Results</a>
                <button class="button-color" onclick="showadvanced()">Show Advanced view</button>
                <button class="button-color" onclick="hideadvanced()">Hide Advanced view</button>
            </div>
        </nav>
    </header>
    <script>
    /* hide class post-author if clicked once and show if clicked again*/
    function showadvanced() {
        var x = document.getElementsByClassName("post-author");
        for (var i = 0; i < x.length; i++) {
            x[i].style.display = "block";
        }
        var x = document.getElementsByClassName("post-status");
        for (var i = 0; i < x.length; i++) {
            x[i].style.display = "block";
        }
        var x = document.getElementsByClassName("post-asseturls");
        for (var i = 0; i < x.length; i++) {
            x[i].style.display = "block";
        }
        var x = document.getElementsByClassName("post-time");
        for (var i = 0; i < x.length; i++) {
            x[i].style.display = "block";
        }
    }
    function hideadvanced() {
        var x = document.getElementsByClassName("post-author");
        for (var i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        var x = document.getElementsByClassName("post-status");
        for (var i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        var x = document.getElementsByClassName("post-asseturls");
        for (var i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        var x = document.getElementsByClassName("post-time");
        for (var i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
    }
    /* copy to clipboard from current class copybutton*/
    </script>

<div class="wrapper">
	<div class="grid">
        """
    for avi in avis:
        idd += 1
        readabletiome = dateparser.parse(avi[0]).strftime("%Y-%m-%d %H:%M:%S")
        if avi[11] == "private":
            statustext = '<b class="" style="color:red;">private &#128308;'
        else:
            statustext = '<b class="" style="color:green;">public &#128994;'
        strr1 = f"""
			<div class="k-card">
					<div class="k-card-body shadow">
						<div class="k-card-images">
                            <img class="k-card-img" src="{sanatize(str(avi[8]))}" alt="Avatar Image" class="k-card-image loading="lazy"">
						</div>
						<div class="k-card-data">
							<span class="post-name">{sanatize(str(avi[2]))}</span>
							<br />
							<span class="post-author">Avatar Author: <b class="">{sanatize(str(avi[5]))}</b></span>
                            <span class="post-avatar_id">Avatar ID: </b><button class="avataridbutton" id="{sanatize(str(idd))}" onclick="copytoclipboard({sanatize(str(idd))})">{sanatize(str(avi[1]))}</button></span>
                            <span class="post-status">STATUS: {statustext}</b></span>
                            PCgfnbfgn
							<div class="post-link">
								<span class="post-time">{sanatize(str(readabletiome))}</span>
							</div>
						</div>
					</div>
				</a>
			</div>
"""
        asstype = ""
        if avi[6] != "None":
            asstype += "PC"
        if asstype != "":
            if avi[7] != "None":
                asstype += " | "
        if avi[7] != "None":
            asstype += "Quest"
        # print(asstype)
        strr1 = strr1.replace("PCgfnbfgn",
                              f"""<span class="post-asseturls">Asset Type(s): {sanatize(str(asstype))}</b></span>""")
        strr += strr1
    strr += """	</div>
</div>
<script>
function copytoclipboard(id) {
    var copyText = document.getElementById(id);
    var txt = copyText.textContent || copyText.innerText;
    fallbackCopyTextToClipboard(txt);
    alert("Copied ID: " + txt);
}

function fallbackCopyTextToClipboard(text) {
    var textArea = document.createElement("textarea");
    textArea.value = text;

    // Avoid scrolling to bottom
    textArea.style.top = "0";
    textArea.style.left = "0";
    textArea.style.position = "fixed";

    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();

    try {
      var successful = document.execCommand('copy');
      var msg = successful ? 'successful' : 'unsuccessful';
      console.log('Fallback: Copying text command was ' + msg);
    } catch (err) {
      console.error('Fallback: Oops, unable to copy', err);
    }

    document.body.removeChild(textArea);
  }
  function copyTextToClipboard(text) {
    if (!navigator.clipboard) {
      fallbackCopyTextToClipboard(text);
      return;
    }
    navigator.clipboard.writeText(text).then(function() {
      console.log('Async: Copying to clipboard was successful!');
    }, function(err) {
      console.error('Async: Could not copy text: ', err);
    });
  }

  var copyBobBtn = document.querySelector('.js-copy-bob-btn'),
    copyJaneBtn = document.querySelector('.js-copy-jane-btn');

  copyBobBtn.addEventListener('click', function(event) {
    copyTextToClipboard('Bob');
  });


  copyJaneBtn.addEventListener('click', function(event) {
    copyTextToClipboard('Jane');
  });
</script>

        </main>
    </div>

    <footer class="footer text-muted">
        <div class="container-fluid margined" style="color: lightgray;">
            &copy; A.R.E.S BiG PEEN <span style="float: right; color: #ef00ff;"
                                                                           id="cn-ct"></span>
        </div>
    </footer>


</body>

</html>"""
    with open("avatars.html", "w", errors="ignore") as f:
        f.write(strr)


def sanatize(sss):
    sss = html.escape(sss)
    return sss