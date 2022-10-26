function displaySuccessNotification(text){
    $("#text-success").text(text);
    $(".ms-notification-success").hide().fadeIn(500).delay(3000).fadeOut(1000);
    $(".ms-notification__close").on("click",function(e){
        e.preventDefault();
        $(".ms-notification-success").hide();
    });
}

function displayErrorNotification(text){
    $("#text-error").text(text);
    $(".ms-notification-error").hide().fadeIn(500).delay(5000).fadeOut(1000);
    $(".ms-notification__close").on("click",function(e){
        e.preventDefault();
        $(".ms-notification-error").hide();
    });
}

function displayWarningNotification(text){
    $("#text-warning").text(text);
    $(".ms-notification-warning").hide().fadeIn(500).delay(5000).fadeOut(1000);
    $(".ms-notification__close").on("click",function(e){
        e.preventDefault();
        $(".ms-notification-warning").hide();
    });
}

