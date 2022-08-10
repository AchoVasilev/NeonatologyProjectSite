$("#file-button i").click(function () {
    $("#upload-file").trigger('click');
});

const errorMsg = "Файлът е прекалено голям!";
alertify.set('notifier', 'position', 'top-center');

const fileUpload = document.getElementById('upload-file');
$(fileUpload).on('change', function () {
    const fileExtensions = ["TXT", "TEXT", "DOCX", "DOC", "PDF", "PPT", "XLS", "XLSX", "ZIP", "RAR"];
    let files = fileUpload.files;
    const dtFiles = new DataTransfer();
    const dtImages = new DataTransfer();

    for (let file of files) {
        let fileExtension = file.name.split('.').pop();
        if (fileExtensions.includes(fileExtension.toUpperCase())) {
            let sizeInMB = (file.size / (1024 * 1024)).toFixed(2);
            if (sizeInMB > 15) {
                alertify.error(errorMsg)
                continue;
            }

            dtFiles.items.add(file);
        } else {
            let sizeInMB = (file.size / (1024 * 1024)).toFixed(2);
            if (sizeInMB > 15) {
                alertify.error(errorMsg)
                continue;
            }

            dtImages.items.add(file);
        }
    }

    fileUpload.files = dtFiles.files;
    let filesCount = fileUpload.files.length;
    let badge = document.querySelector("span.select-file-badge");
    badge.innerText = filesCount;

    if (filesCount > 0) {
        badge.style.boxShadow = "0 0 0 0 rgba(2, 191, 255, 1)";
        badge.style.animation = "pulse-green 2s infinite";
    } else {
        badge.style.boxShadow = "";
        badge.style.animation = "";
    }

    if (dtImages.items.length > 0) {
        transferImages(dtImages);
    }
});

function transferFiles(dtFiles) {
    const fileExtensions = ["TXT", "TEXT", "DOCX", "DOC", "PDF", "PPT", "XLS", "XLSX", "ZIP", "RAR"];
    let files = fileUpload.files;
    const dt = new DataTransfer();

    for (let file of files) {
        let fileExtension = file.name.split('.').pop();
        if (fileExtensions.includes(fileExtension.toUpperCase())) {
            dt.items.add(file);
        }
    }

    for (let file of dtFiles.files) {
        let fileExtension = file.name.split('.').pop();
        let isFileExist = [...files].some(x => x.name == file.name);
        if (fileExtensions.includes(fileExtension.toUpperCase()) && !isFileExist) {
            dt.items.add(file);
        }
    }

    fileUpload.files = dt.files;
    let filesCount = fileUpload.files.length;
    let badge = document.querySelector("span.select-file-badge");
    badge.innerText = filesCount;

    if (filesCount > 0) {
        badge.style.boxShadow = "0 0 0 0 rgba(2, 191, 255, 1)";
        badge.style.animation = "pulse-green 2s infinite";
    } else {
        badge.style.boxShadow = "";
        badge.style.animation = "";
    }
}