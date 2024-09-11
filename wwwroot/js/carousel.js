function initializeCarousel() {

    const prev = document.querySelector('.prev');
    const next = document.querySelector('.next');
    const track = document.querySelector('.track');
    let carouselWidth = document.querySelector('.carousel-container').offsetWidth;
    window.addEventListener('resize', () => {
        carouselWidth = document.querySelector('.carousel-container').offsetWidth;
    })
    let index = 0;

    next.addEventListener('click', () => {
        index++;
        prev.classList.add('show');
        track.style.transform = `translateX(-${index * carouselWidth + 15}px)`;
        if (track.offsetWidth - (index * carouselWidth + 16) < carouselWidth) {
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
            track.style.transform = `translateX(-${index * carouselWidth + 15}px)`;
        }
    })
}