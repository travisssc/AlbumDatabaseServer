function initializeCarousel(carouselId) {
    const carouselContainer = document.getElementById(`carousel-${carouselId}`);
    const prev = document.getElementById(`prev-${carouselId}`);
    const next = document.getElementById(`next-${carouselId}`);
    const track = document.getElementById(`track-${carouselId}`);
    let carouselWidth = document.querySelector('.carousel-container').offsetWidth;
    window.addEventListener('resize', () => {
        carouselWidth = document.querySelector('.carousel-container').offsetWidth;
    })
    let index = 0;

    next.addEventListener('click', () => {
        index++;
        prev.classList.add('show');
        track.style.transform = `translateX(-${index * carouselWidth + (15 * index)}px)`;
        if (track.offsetWidth - (index * carouselWidth + 400) < carouselWidth) { //no idea why 400 works should probably fix soon!
            next.classList.add('hide');
        }
    })

    prev.addEventListener('click', () => {
        index--;
        next.classList.remove('hide');
        if (index == 0) {
            prev.classList.remove('show');
            track.style.transform = `translateX(-${index * carouselWidth}px)`;
        }
        else {
            track.style.transform = `translateX(-${index * carouselWidth + 15 * (index)}px)`;
        }
    })
}