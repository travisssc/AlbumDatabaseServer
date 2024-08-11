const API_KEY = 'AIzaSyDPW-O0nTfZUPVpPuL4Q3H0HR8d2AIpDJc'
const CHANNEL_ID = 'UCBv7HEHuVlNAELGi5XJd85Q'

async function fetchLatestVideo() {
    const url = `https://www.googleapis.com/youtube/v3/search?key=${API_KEY}&channelId=${CHANNEL_ID}&order=date&part=snippet&type=video&maxResults=5`
    const response = await fetch(url)
    const data = await response.json()
    for (let item of data.items) {
        const videoDetails = await fetch(`https://www.googleapis.com/youtube/v3/videos?part=contentDetails&id=${item.id.videoId}&key=${API_KEY}`)
        const videoData = await videoDetails.json()
        const duration = videoData.items[0].contentDetails.duration
        if (duration.includes('M')) {
            document.getElementById('youtube-video').innerHTML =
                '<iframe width="560" height="315" src = "https://www.youtube.com/embed/' + item.id.videoId + '" frameborder = "0" allowfullscreen></iframe >'
            break
        }   
    }
}

fetchLatestVideo();