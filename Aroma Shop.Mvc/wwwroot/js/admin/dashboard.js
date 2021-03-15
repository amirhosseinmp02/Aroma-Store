//For DropDownMenu
$(document).ready(function () {
    $(".dropdown-btn").click(function () {
        $(".dropdown-container").toggle(500);
    });
});



const dt = new DataTransfer(); // Permet de manipuler les fichiers de l'input file

$("#attachment").on('change', function (e) {
    for (var i = 0; i < this.files.length; i++) {
        let fileBloc = $('<span/>', { class: 'file-block' }),
            fileName = $('<img/>', { class: 'productimages', src: URL.createObjectURL(this.files.item(i)), alt: this.files.item(i).name });
        fileBloc.append('<span class="file-delete badge badge-danger"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x" viewBox="0 0 16 16">\r\n  <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"/>\r\n</svg></span>')
            .append(fileName);
        $("#filesList > #files-names").append(fileBloc);
        //document.getElementById("lol").src = URL.createObjectURL(this.files.item(i));
    };
    // Ajout des fichiers dans l'objet DataTransfer
    for (let file of this.files) {
        dt.items.add(file);
    }
    // Mise à jour des fichiers de l'input file après ajout
    this.files = dt.files;

    // EventListener pour le bouton de suppression créé
    $('span.file-delete').click(function () {
        let name = $(this).next('img.productimages').attr("alt");
        // Supprimer l'affichage du nom de fichier
        $(this).parent().remove();
        for (let i = 0; i < dt.items.length; i++) {
            // Correspondance du fichier et du nom
            if (name === dt.items[i].getAsFile().name) {
                // Suppression du fichier dans l'objet DataTransfer
                dt.items.remove(i);
                continue;
            }
        }
        // Mise à jour des fichiers de l'input file après suppression
        document.getElementById('attachment').files = dt.files;
    });
});



//(function() {
//    'use strict'

//    feather.replace()

//    // Graphs
//    var ctx = document.getElementById('myChart')
//    // eslint-disable-next-line no-unused-vars
//    var myChart = new Chart(ctx,
//        {
//            type: 'line',
//            data: {
//                labels: [
//                    'Sunday',
//                    'Monday',
//                    'Tuesday',
//                    'Wednesday',
//                    'Thursday',
//                    'Friday',
//                    'Saturday'
//                ],
//                datasets: [
//                    {
//                        data: [
//                            15339,
//                            21345,
//                            18483,
//                            24003,
//                            23489,
//                            24092,
//                            12034
//                        ],
//                        lineTension: 0,
//                        backgroundColor: 'transparent',
//                        borderColor: '#007bff',
//                        borderWidth: 4,
//                        pointBackgroundColor: '#007bff'
//                    }
//                ]
//            },
//            options: {
//                scales: {
//                    yAxes: [
//                        {
//                            ticks: {
//                                beginAtZero: false
//                            }
//                        }
//                    ]
//                },
//                legend: {
//                    display: false
//                }
//            }
//        })
//})()