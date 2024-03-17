$(".tweets-search").click(function () {
    var uname = $(this).data("name");
    $.ajax({
        type: "GET",
        url: `/Tweets/SearchTweets?username=` + uname,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $("#tweet-results").html(data);
        }
    });
});

function AddTweetSuccess(response) {
    alert(response);
}

async function Retweet() {
    var dataContent = {
        pin: $("#pin").val(),
        token: $('#oauthtoken').val(),
        tweetId: $('#tweetId').val()
    }
    var jsonData = await JSON.stringify(dataContent);
    $.ajax({
        type: "POST",
        url: `/LocalDBTweets/Retweet`,
        contentType: "application/json",
        dataType: "json",
        data: jsonData,
        success: function (data) {
            $('#myModal').removeClass('show');
            $('#myModal').addClass('hide');
            alert(data);
        },
        error: function () {
            $('#myModal').removeClass('show');
            $('#myModal').addClass('hide');
        }
    });
}

async function RequestTokenSuccess(response) {
    await window.open('https://api.twitter.com/oauth/authorize?' + response.tokenData);
    $('#oauthtoken').val(response.tokenData);
    $('#tweetId').val(response.tweetId);
    $('#myModal').modal('show');
}
