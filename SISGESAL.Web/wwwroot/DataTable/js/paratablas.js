        $(document).ready(function () {
            $('#tabla').DataTable({
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json"
                },
                //responsive: true,
                dom: '<"pt-2 row" <"col-xl"l><"col-x"B><"col-xl"f> >rtip',
                buttons: [

                    {
                        extend: 'copy',
                        text: ' Copiar',
                        className: 'btn btn-info fa fa-solid fa-copy'
                    },
                    {
                        extend: 'print',
                        text: ' Imprimir',
                        className: 'btn btn-secondary fa fa-solid fa-print'
                    },
                    {
                        extend: 'pdfHtml5',
                        text: ' PDF',
                        className: 'btn btn-danger fa fa-solid fa-file',
                        orientation: 'portrait',
                        pageSize: 'FOLIO',
                        // customize: function (doc) {
                        //     doc.content.splice(1, 0, {
                        //         margin: [0, 0, 0, 12],
                        //         alignment: 'center',
                        //         image: '~/Imagenes/PODERJUDICIAL.png',
                        //         width: 95,
                        //         height: 40
                        //     });
                        // }
                    },
                    {
                        extend: 'excel',
                        text: ' Excel',
                        className: 'btn btn-success fa fa-solid fa-file'
                    }
                ]
            });
        });