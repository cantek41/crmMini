
$(document).ready(function () {
    $("#COMPANY_CODE").change(function () {    
        $('#PROJECT').empty();
        $.ajax({
            url: "/Activity/filterContact",
            type: 'POST',
            data: { CompanyCode: $('#COMPANY_CODE').val() },
            datatype: 'json',
            success: function (data) {
                var elements = "<option  value='0'>' -- '</option>'";
                $.each(data, function () {
                    elements = elements + '<option  value=' + this.id + '>' + this.state + '</option>'
                })
                $('#CONTACT_CODE').empty().attr('disabled', false).append(elements);
            }
        });

        $.ajax({
            url: "/Activity/projeGetir",
            type: 'POST',
            data: { company: $('#COMPANY_CODE').val() },
            datatype: 'json',
            success: function (data) {
                var elements = "<option  value=-1>Seçiniz</option>";
                $.each(data, function () {
                    elements += '<option  value=' + this.id + '>' + this.state + '</option>'
                })
                $('#PROJECT').empty().attr('disabled', false).append(elements);
            }
        });
    });
});