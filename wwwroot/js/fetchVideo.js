const API_KEY = 'AIzaSyDPW-O0nTfZUPVpPuL4Q3H0HR8d2AIpDJc'
const CHANNEL_ID = 'UCBv7HEHuVlNAELGi5XJd85Q'

async function fetchLatestVideo() {
    const url = `https://www.googleapis.com/youtube/v3/search?key=${API_KEY}&channelId=${CHANNEL_ID}&order=date&part=snippet&type=video&maxResults=5`
    try {
        const response = await fetch(url)
        if (!response.ok) {
            throw new Error('Failed to fetch data: ${response.statusText}')
        }
        const data = await response.json()
        if (!data.items || data.items.length == 0) {
            throw new Error('No videos found');
        }
        for (let item of data.items) {
            const videoDetails = await fetch(`https://www.googleapis.com/youtube/v3/videos?part=contentDetails&id=${item.id.videoId}&key=${API_KEY}`)
            const videoData = await videoDetails.json()
            if (!videoData.items || videoData.items.length == 0) {
                throw new Error('Video details not found.')
            }
            const duration = videoData.items[0].contentDetails.duration
            if (duration.includes('M')) {
                document.getElementById('youtube-video').innerHTML =
                    '<iframe width="560" height="315" src = "https://www.youtube.com/embed/' + item.id.videoId + '" frameborder = "0" allowfullscreen></iframe >'
                break
            }
        }
    } catch (error) {
        console.error('Error fetching video:', error.message)
    }
    
}

fetchLatestVideo();