$(function () {
    $("#ulke").val("TÜRKİYE");
    ilgetir();
});
function ilgetir() {
    var il = $('#ulke').val();
    
    $('#iller').empty().append('<option  value="-1">Lütfen Seçiniz</option>');
    $.ajax({
        url: "/Company/getil",
        type: 'POST',
        data: { ulkeName: $("#ulke").val() },
        datatype: 'json',
        success: function (data) {
            var elements = "";
            $.each(data, function () {
                elements = elements + '<option  value=' + this.state + '>' + this.state + '</option>'
            })
            $('#iller').attr('disabled', false).append(elements);
        }
    });
    $('#iller').val(il);
}

function ilcegetir() {
    var ilce = $('#iller').val();
    $('#ilce').empty();
    $('#ilce').append('<option  value="-1">Lütfen Seçiniz</option>');
    $.ajax({
        url: "/Company/getilce",
        type: 'POST',
        data: { cityname: $("#iller").val() },
        datatype: 'json',
        success: function (data) {
            var elements = "";
            $.each(data, function () {
                elements = elements + '<option  value=' + this.state + '>' + this.state + '</option>'
            })
            $('#ilce').attr('disabled', false).append(elements);
        }
    });
    $('#ilce').val(ilce);
}