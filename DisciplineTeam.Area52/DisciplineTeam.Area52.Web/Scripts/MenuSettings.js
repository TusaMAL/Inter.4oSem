function openTab(evt, tabName) {
        var i, x, tablinks;
        x = document.getElementsByClassName("tab-set");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        tablinks = document.getElementsByClassName("tablink");
        for (i = 0; i < x.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" botao-settings-select", "");
        }
        document.getElementById(tabName).style.display = "block";
        evt.currentTarget.className += " botao-settings-select";
 }
