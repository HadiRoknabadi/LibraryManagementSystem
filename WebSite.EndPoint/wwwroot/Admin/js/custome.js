function open_waiting(selector = 'body') {
    $(selector).waitMe({
        effect: 'facebook',
        text: 'لطفا صبر کنید ...',
        bg: 'rgba(255,255,255,0.7)',
        color: '#000'
    });
}

function close_waiting(selector = 'body') {
    $(selector).waitMe('hide');
}

/****** start CK Editor ******/
$(document).ready(function () {

    var editors = $("[ckeditor]");
    if (editors.length > 0) {
        open_waiting();

        $.getScript('https://static.webcafeyar.ir/lib/ckeditor/ckeditor.js', function () {


            $(editors).each(function (index, value) {

                var id = $(value).attr('ckeditor');
                ClassicEditor.create(document.querySelector('[ckeditor="' + id + '"]'),
                    {


                        language: 'fa',
                        image: {
                            toolbar: [
                                'imageTextAlternative',
                                'imageStyle:inline',
                                'imageStyle:block',
                                'imageStyle:side'
                            ]
                        },
                        table: {
                            contentToolbar: [
                                'tableColumn',
                                'tableRow',
                                'mergeTableCells'
                            ]
                        },
                        licenseKey: '',
                        simpleUpload: {
                            // The URL that the images are uploaded to.
                            uploadUrl: '/CkEditorImageUploader/UploadImage'
                        }


                    })
                    .then(editor => {
                        window.editor = editor;




                    })
                    .catch(error => {
                        console.error('Oops, something went wrong!');
                        console.error('Please, report the following error on https://github.com/ckeditor/ckeditor5/issues with the build id and the error stack trace:');
                        console.warn('Build id: f443yzl101re-knhuhha922ax');
                        console.error(error);
                    });
            });
            close_waiting();

        });

    }

});
/****** end CK Editor ******/


function FillPageId(pageId) {
    $("#PageId").val(pageId);
    $("#filter-form").submit();
}

function ImagePreview(imageId, imagePreviewId = 0) {
    document.getElementById(imageId).onchange = function () {
        var reader = new FileReader();

        reader.onload = function (e) {
            // get loaded data and render thumbnail.
            if (imagePreviewId == 0) {
                document.getElementById("ImagePreview").src = e.target.result;
            }
            else {
                document.getElementById(imagePreviewId).src = e.target.result;
            }

        };

        // read the image file as a data URL.
        reader.readAsDataURL(this.files[0]);
    };
}
function ShowToastMessageNotification(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-right',
        showDuration: 4000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
    });
}


function ShowSweetAlertNotification(title, text, iconName) {
    swal({
        title: title !== '' ? title : 'اعلان',
        icon: iconName !== '' ? iconName : 'success',
        text: decodeURI(text)
    });
}



$('[ajax-url-button]').on('click', function (e) {

    e.preventDefault();

    var url = $(this).attr('href');

    IsHide = $(this).attr('Hide');

    var itemId = $(this).attr('ajax-url-button');
    swal({
        title: 'اخطار',
        text: "آیا از انجام عملیات مورد نظر اطمینان دارید؟",
        icon: "warning",
        buttons: {
            catch: {
                text: "بله",
                value: "catch",
            },
            cancel: "خیر",

        },
    }).then((value) => {
        switch (value) {
            case "catch":
                $.get(url).then(result => {
                    switch (result.status) {
                        case "Success":
                            ShowToastMessageNotification('موفقیت', result.message);
                            if (IsHide == true || IsHide == null) {
                                $('#ajax-url-item-' + itemId).hide(1500);

                            }
                            else {
                                location.reload();
                            }
                            break;

                        case "Warning":
                            ShowToastMessageNotification('خطا', result.message, 'warning');
                            break;

                        case "Error":
                            ShowToastMessageNotification('خطا', result.message, 'error');
                            break;

                    }

                });
                break;
            default:
                swal({
                    title: 'پیغام',
                    text: 'عملیات لغو شد',
                    icon: "error",
                    buttons: 'بسیارخوب'
                });
                break;

        }
    });

});

function OnSuccessAddOrEditItem(res) {
    switch (res.status) {
        case 'Success':
            ShowToastMessageNotification('اعلان موفقیت', res.message);
            $('.close').click();
            setTimeout(function () { location.reload(); }, 2000)
            break;

        case 'Error':
            ShowToastMessageNotification('اخطا', res.message, 'error');
            $('.close').click();
            break;

        case 'Warning':
            ShowToastMessageNotification('خطا', res.message, 'warning');
            break;
    }
}

function OnSuccessOpenCloseCafe(res) {
    switch (res.status) {
        case 'Success':
            ShowToastMessageNotification('اعلان موفقیت', res.message);
            $('.close').click();
            break;

        case 'Error':
            ShowToastMessageNotification('اخطا', res.message, 'error');
            $('.close').click();
            break;

        case 'Warning':
            ShowToastMessageNotification('خطا', res.message, 'warning');
            $('.close').click();
            break;
    }
}

/****** start product scripts ******/

$("[main_category_checkbox]").on('change', function (e) {

    isChecked = $(this).is(':checked');
    var selectedCategoryId = $(this).attr('main_category_checkbox');


    if (isChecked) {

        $('#sub_categories_' + selectedCategoryId).slideDown(300);

    }
    else {

        $('#sub_categories_' + selectedCategoryId).slideUp(300);
        var subCategories = $('[parent-category-id="' + selectedCategoryId + '"]');
        $('[parent-category-id="' + selectedCategoryId + '"]').prop('checked', false);
    }
});



function CheckImageIsNull(formId, btnSubmitId, fileInputId, name) {
    $("#" + btnSubmitId).on('click', function () {

        var fileInput = $("#" + fileInputId).val();
        if (fileInput == '') {
            ShowToastMessageNotification('اخطار', `لطفا تصویری را برای ${name} انتخاب کنید`, 'warning');
            return false;
        }


        $("#" + formId).submit();

    });

}



