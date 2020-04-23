// 2. This code loads the IFrame Player API code asynchronously.
  var tag = document.createElement('script');

  tag.src = "https://www.youtube.com/iframe_api";
  var firstScriptTag = document.getElementsByTagName('script')[0];
  firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

  // 3. This function creates an <iframe> (and YouTube player)
  //    after the API code downloads.
  var player;
  var autoPlay = false;
  var quality = "hd720";
  var startUpId = 'SJdfQB289Zc';
  
  function onYouTubeIframeAPIReady() {
	player = new YT.Player('player', {
	  height: '390',
	  width: '640',
	  videoId: startUpId,
      suggestedQuality: "hd720",
		playerVars: {
		  'autoplay': 1,
          'controls': 0,
          'showinfo': 0
      },
	  events: {
		'onReady': onPlayerReady,
		'onStateChange': onPlayerStateChange,
		'onPlaybackQualityChange': onPlayerPlaybackQualityChange,
		'onPlaybackRateChange': onPlaybackRateChange,
		'onError': onError,
		'onApiChange': onApiChange
	  }
	});
  }

function onPlayerReady(event) {
    console.log("onplayer ready event:"+event)
	boundAsync.playerLoaded();
}

function onPlayerStateChange(event) {

    console.log("onplayer state changed event:" + event)
	boundAsync.playingChanged(event.data);
}
  
function onPlayerPlaybackQualityChange(event) {

	console.log("onplayer playback qty changed event:" + event)
	boundAsync.qualityChanged();
}
function onPlaybackRateChange(event) {

	console.log("onplayer playback rate changed event:" + event)
	boundAsync.rateChanged(event.data);
}
function onError(event) {

	console.log("onplayer playback changed event:" + event)
	boundAsync.onError(event.data);
}
function onApiChange(event) {

	console.log("onplayer playback changed event:" + event)
	boundAsync.onApiChange(event.data);
}

function setPlayerState(state) {
	if(state == "stop")
		player.stopVideo();
	else if (state == "start")
		player.playVideo();
	else if(state == "pause")
		player.pauseVideo();
	
}
  
function setVolume(volume) {
	console.log("on set volume:" + volume)
	player.setVolume(volume);
}

function setVideoId(videoId) {
	console.log("on set videoid:" + videoId)
	if(autoPlay){
		player.loadVideoById(videoId,0,quality);
	}
	else{
		player.cueVideoById(videoId,0,quality);
	}
}

function setQuality(quality) {
	console.log("on set quality:" + quality)
	player.setPlaybackQuality(quality);
}

function seekTo(startSeconds) {
	console.log("on seekto secs:" + startSeconds)
	player.seekTo(startSeconds, true);
}

function pauseVideo() {
	player.pauseVideo();
}

function playVideo() {
	player.playVideo();
}

function loadVideo(videoId, startSeconds) {
	player.loadVideoById(videoId, startSeconds);
	boundAsync.sendVideoId(videoId);
}

function cueVideo(videoId, startSeconds) {
	player.cueVideoById(videoId, startSeconds);
	boundAsync.sendVideoId(videoId);
}

function mute() {
	player.mute();
}

function unMute() {
	player.unMute();
}

(async function () {
	await CefSharp.BindObjectAsync("boundAsync", "boundAsync");
	boundAsync.showMessage('Message from JS');
})();