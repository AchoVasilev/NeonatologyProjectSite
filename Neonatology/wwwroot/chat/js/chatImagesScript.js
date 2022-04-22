$("#image-button i").click(function () {
    $("#upload-image").trigger('click');
});

const errorImgMsg = "Файлът е прекалено голям!";
alertify.set('notifier', 'position', 'top-center');

const imageUpload = document.getElementById('upload-image');
$(imageUpload).on('change', function () {
    const imageExtensions = ["JPG", "PNG", "JPEG"];
    let files = imageUpload.files;
    const dtImages = new DataTransfer();
    const dtFiles = new DataTransfer();

    for (let file of files) {
        let fileExtension = file.name.split('.').pop();
        if (imageExtensions.includes(fileExtension.toUpperCase())) {
            let sizeInMB = (file.size / (1024 * 1024)).toFixed(2);
            if (sizeInMB > 15) {
                alertify.error(msg)
                continue;
            }

            dtImages.items.add(file);
        } else {
            let sizeInMB = (file.size / (1024 * 1024)).toFixed(2);
            if (sizeInMB > 15) {
                alertify.error(msg)
                continue;
            }

            dtFiles.items.add(file);
        }
    }

    imageUpload.files = dtImages.files;
    let filesCount = imageUpload.files.length;
    let badge = document.querySelector("span.select-image-badge");
    badge.innerText = filesCount;

    if (filesCount > 0) {
        badge.style.boxShadow = "0 0 0 0 rgba(51, 217, 178, 1)";
        badge.style.animation = "pulse-green 2s infinite";
    } else {
        badge.style.boxShadow = "";
        badge.style.animation = "";
    }

    if (dtFiles.items.length > 0) {
        transferFiles(dtFiles);
    }
});

function transferImages(dtImages) {
    const imageExtensions = ["JPG", "PNG", "JPEG"];
    let images = imageUpload.files;
    const dt = new DataTransfer();

    for (let image of images) {
        let fileExtension = image.name.split('.').pop();
        if (imageExtensions.includes(fileExtension.toUpperCase())) {
            dt.items.add(image);
        }
    }

    for (let image of dtImages.files) {
        let fileExtension = image.name.split('.').pop();
        let isFileExist = [...images].some(x => x.name == image.name);
        if (imageExtensions.includes(fileExtension.toUpperCase()) && !isFileExist) {
            dt.items.add(image);
        }
    }

    imageUpload.files = dt.files;
    let imagesCount = imageUpload.files.length;
    let badge = document.querySelector("span.select-image-badge");
    badge.innerText = imagesCount;

    if (imagesCount > 0) {
        badge.style.boxShadow = "0 0 0 0 rgba(2, 191, 255, 1)";
        badge.style.animation = "pulse-green 2s infinite";
    } else {
        badge.style.boxShadow = "";
        badge.style.animation = "";
    }
}