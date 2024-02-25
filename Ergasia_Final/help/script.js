window.onload = function () { 
    // Add click event to expanding sections in sidebar
    var expanders = document.getElementsByClassName("menu-item")
    for (var i = 0; i < expanders.length; i++) {
        var expander = expanders[i]
        expander.addEventListener("click", function () {
            if (expander.classList.contains("active")) {
                expander.classList.remove("active")
            } else {
                expander.classList.add("active")
            }
        });
    }

    
}