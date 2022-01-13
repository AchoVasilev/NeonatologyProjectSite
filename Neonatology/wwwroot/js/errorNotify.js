function notify(msg) {
    const element = document.getElementById('errorBox');
    const output = element.querySelector('span');
    output.textContent = msg;
    element.style.display = 'block';

    setTimeout(() => element.style.display = 'none', 3000);
}