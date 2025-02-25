// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets. josh

$(document).ready(function () {
    const studentId = 0;

    function loadCourses() {
        $.ajax({
            url: '/Course/GetAllCourses', 
            type: 'GET',
            dataType: 'json',
            success: function (courses) {
                var courseSelect = $('#course');
                courseSelect.empty(); 
                courseSelect.append('<option value="">Seleccione un Curso</option>');
                courses.forEach(function (course) {
                    courseSelect.append(
                        `<option value="${course.id}">${course.name}</option>`
                    );
                });
            },
            error: function (error) {
                console.log('Error al cargar los cursos: ', error);
            }
        });
    }

    // Llamar a la función para cargar los cursos al cargar la página
    loadCourses();

    loadConsultations();
    // Función para cargar las consultas activas
    function loadConsultations() {
        $.ajax({
            url: "/consult/GetActiveConsults", // Ruta para obtener consultas activas
            type: "GET",
            success: function (data) {
                if (data && data.length > 0) {
                    let consultationsContent = '';

                    data.forEach(function (consult) {
                        // Solo mostramos la descripción y la fecha
                        consultationsContent += `
                    <div class="consultation-item" data-consult-id="${consult.id}">
                        <h3>Descripción: ${consult.descriptionConsult}</h3>
                        <p><strong>Fecha:</strong> ${new Date(consult.dateconsult).toLocaleDateString()}</p>
                    </div>
                    `;
                    });

                    $("#consultations").html(consultationsContent); // Insertar las consultas en el contenedor
                } else {
                    $("#consultations").html("<p>No hay consultas activas.</p>");
                }
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Error",
                    text: "No se pudieron cargar las consultas.",
                });
            }
        });
    }




    // Mostrar el formulario del foro
    $("#consultForm").submit(function (event) {
        event.preventDefault();

        // Obtener datos del formulario
        const description = $("#description").val().trim();
        const isUrgent = $("#type").prop("checked");
        // const authorId = $("#author").val();

         const course = $("#course").val();
        const date = $("#date").val();
        const isActive = $("#status").prop("checked");

        const isPublic = $("#public").prop("checked");
        const isPrivate = $("#private").prop("checked");

        // Validar que solo uno de los checkboxes (Publica o Privada) esté seleccionado
        if (isPublic && isPrivate) {
            Swal.fire({
                icon: "warning",
                title: "Selección Incorrecta",
                text: "Solo puedes seleccionar un tipo de consulta: Publica o Privada.",
            });
            return;
        }

        //const isUrgent = null;

        //if (isPublic) {
        //    isUrgent = true;
        //} else if (isPrivate){
        //    isUrgent = false;
        //}
      const newConsultation = {
            id: 0,
            idcourse: course,
            descriptionConsult: description,
            typeconsult: true,
            author: 13,
            dateconsult: date,
            statusconsult: true,
        };

        if (isPublic) {
            newConsultation.typeconsult = true;
        } else if (isPrivate) {
            newConsultation.typeconsult = false;
        }

        if (description === "" || date === "") {
            Swal.fire({
                icon: "warning",
                title: "Campos Vacíos",
                text: "Todos los campos son obligatorios.",
            });
            return;
        }

        // Crear la consulta
       
   
        // Aquí se puede realizar una llamada AJAX para enviar la consulta al servidor
        $.ajax({
            url: "/Consult/SubmitConsult",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(newConsultation),
            success: function () {
                Swal.fire({
                    icon: "success",
                    title: "Consulta Enviada",
                    text: "La consulta se ha enviado correctamente.",
                });

                // Limpiar formulario
                $("#consultForm")[0].reset();
                loadConsultations();
                // Cargar las consultas nuevamente
                //loadConsultations();
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Error",
                    text: "No se pudo enviar la consulta.",
                });
            }
        });
    });

    // Show login modal on page load
    $('#loginModal').fadeIn();

    // Close modal when close button is clicked
    $('#closeModal').click(function () {
        $('#loginModal').fadeOut();
    });

    // Switch to Login Tab
    $('#loginTab').click(function () {
        $('#loginFormContainer').show();
        $('#registerFormContainer').hide();
        $(this).addClass('active');
        $('#registerTab').removeClass('active');
    });

    // Switch to Register Tab
    $('#registerTab').click(function () {
        $('#registerFormContainer').show();
        $('#loginFormContainer').hide();
        $(this).addClass('active');
        $('#loginTab').removeClass('active');
    });

    // Evento para el formulario de login 
    $("#loginForm").submit(function (event) {
        event.preventDefault();

        const studentData = {
            userName: $("#username").val(),
            password: $("#password").val()
        };

        $.ajax({
            url: "/Student/Login",  //  Ahora usa POST
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(studentData), //  Enviar el objeto JSON
            success: function (response) {
                Swal.fire({
                    icon: "success",
                    title: "Inicio de sesión exitoso",
                    text: "Bienvenido, " + response.name
                }).then(() => {
                    localStorage.setItem("loggedUser", JSON.stringify(response)); // 
                    $("#loginModal").fadeOut();
                    mostrarPerfil(); 
                });
            },
            error: function (xhr) {
                let errorMessage = "Usuario o contraseña incorrectos";
                if (xhr.status === 404) {
                    errorMessage = "El usuario no existe.";
                } else if (xhr.status === 401) {
                    errorMessage = "Contraseña incorrecta.";
                }
                Swal.fire({ icon: "error", title: "Error de autenticación", text: errorMessage });
            }
        });
    });

    function mostrarPerfil() {
        console.log("Ejecutando mostrarPerfil()"); //  Verifica si la función se ejecuta

        let user = JSON.parse(localStorage.getItem("loggedUser"));

        if (user) {
            console.log("Datos del usuario:", user); //  Verifica qué datos se están obteniendo

            $("#profile-name").text(user.name + " " + user.lastName);
            $("#profile-pic").attr("src", user.photo ? "data:image/png;base64," + user.photo : "/images/default-user.png");
            $(".profile-container").fadeIn(); //  Mostrar perfil con animación
        } else {
            console.log("❌ No se encontró usuario en localStorage");
        }
    }


    $(document).ready(function () {
        mostrarPerfil(); // Cargar perfil si ya hay sesión activa
    });

    // Evento para el formulario de registro
    $("#registerForm").submit(function (event) {
        event.preventDefault();
        registrarEstudiante(); // Function for registration logic
    });

    // Initialize other components on page load
    loadProfessors();  // Function to load professors' info
    loadNewsAndComments();  // Function to load news and comments

    // Handle student registration form
    $("#studentForm").submit(function (event) {
        event.preventDefault();
        registrarEstudiante(); // Function to handle student registration
    });

    // Event handler for "Ver comentarios" button (display comments)
    $(document).on("click", ".view-comments", function () {
        const newsId = $(this).data("news-id");

        // Show the modal
        $("#commentsModal").show();

        // Fetch and display comments
        fetchComments(newsId);
    });

    // Function to fetch and display comments
    function fetchComments(newsId) {
        $.ajax({
            url: `/Comment/GetCommentsByNewsId/${newsId}`,
            type: "GET",
            success: function (data) {
                let commentsContent = "";

                if (data.length === 0) {
                    commentsContent = "<p>No hay comentarios aún.</p>";
                } else {
                    data.forEach(comment => {
                        commentsContent += `
                        <div class="comment-item">
                            <p>${comment.description}</p>
                            <span class="comment-date">${new Date(comment.commentDate).toLocaleDateString()}</span>
                        </div>
                    `;
                    });
                }

                $("#commentsList").html(commentsContent);
                $("#submitComment").data("news-id", newsId); // Store newsId for later use
            }
        });
    }

    // Event handler for adding a new comment
    $("#submitComment").on("click", function () {
        const newsId = $(this).data("news-id");
        const description = $("#newComment").val().trim();

        if (description === "") {
            Swal.fire({
                icon: "warning",
                title: "Advertencia",
                text: "El comentario no puede estar vacío.",
            });
            return;
        }

        const newComment = {
            idNews: newsId,
            description: description,
            author: 13
        };

        // Submit the comment
        submitComment(newsId, newComment);
    });

    // Function to submit a new comment
    function submitComment(newsId, newComment) {
        $.ajax({
            url: "/Comment/InsertComment",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(newComment),
            success: function () {
                Swal.fire({
                    icon: "success",
                    title: "Éxito",
                    text: "Comentario agregado correctamente.",
                });

                // Refresh comments
                fetchComments(newsId);

                // Clear the input field
                $("#newComment").val("");
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Error",
                    text: "No se pudo agregar el comentario.",
                });
            }
        });
    }
});

$(document).ready(function () {

    // Event handler for "Ver comentarios" button (display comments)
    $(document).on("click", ".view-comments", function () {
        const consultId = $(this).data("consult-id");

        // Show the modal
        $("#commentsModal").show();

        // Fetch and display comments
        fetchComments(consultId);
    });

    // Function to fetch and display comments
    function fetchComments(consultId) {
        $.ajax({
            url: `/CommentConsult/GetCommentsByConsultId/${consultId}`,  // Reemplaza con la URL adecuada
            type: "GET",
            success: function (data) {
                let commentsContent = "";

                if (data.length === 0) {
                    commentsContent = "<p>No hay comentarios aún.</p>";
                } else {
                    data.forEach(comment => {
                        commentsContent += `
                        <div class="comment-item">
                            <p>${comment.descriptionComment}</p>
                            <span class="comment-date">${new Date(comment.commentDate).toLocaleDateString()}</span>
                        </div>
                    `;
                    });
                }

                // Insert the comments into the modal
                $("#commentsList").html(commentsContent);
                $("#submitComment").data("consult-id", consultId);  // Store consultId for later use
            }


        });
    }

    // Event handler for adding a new comment
    $("#submitComment").on("click", function () {
        const consultId = $(this).data("consult-id");
        const description = $("#newComment").val().trim();

        if (description === "") {
            Swal.fire({
                icon: "warning",
                title: "Advertencia",
                text: "El comentario no puede estar vacío.",
            });
            return;
        }

        const newComment = {
            idConsult: consultId,
            descriptionComment: description,
            author: 13  // Cambia el ID de autor según sea necesario
        };

        // Submit the comment
        submitComment(consultId, newComment);
    });

    // Function to submit a new comment
    function submitComment(consultId, newComment) {
        $.ajax({
            url: "/CommentConsult/InsertCommentConsult",  // Reemplaza con la URL adecuada
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(newComment),
            success: function () {
                Swal.fire({
                    icon: "success",
                    title: "Éxito",
                    text: "Comentario agregado correctamente.",
                });

                // Refresh comments
                fetchComments(consultId);

                // Clear the input field
                $("#newComment").val("");
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Error",
                    text: "No se pudo agregar el comentario.",
                });
            }
        });
    }

    // Close modal when the user clicks the close button
    $(".close-btn").on("click", function () {
        $("#commentsModal").hide();  // Hide the modal
    });

    // Close modal when the user clicks outside the modal content
    $(window).on("click", function (event) {
        if ($(event.target).is("#commentsModal")) {
            $("#commentsModal").hide();  // Hide the modal if clicked outside
        }
    });

});



//////////


// Función para obtener datos del formulario
function obtenerDatosFormulario() {
    let studentData = {
        name: $("#name").val(),
        lastName: $("#lastName").val(),
        email: $("#email").val(),
        userName: $("#userName").val(),
        password: $("#passwordRegister").val(),
        socialLinks: $("#socialLinks").val(),
        statusStudent: $("#statusStudent").val() === "true"
    };

    console.log("Valor de #password:", $("#registerForm #password").val());

    console.log(" Datos extraídos del formulario:", studentData); 

    return studentData;
}


// Función para mostrar alertas con SweetAlert2
function mostrarAlerta(tipo, titulo, mensaje) {
    Swal.fire({
        icon: tipo,
        title: titulo,
        text: mensaje
    });
}

// Función para limpiar el formulario
function limpiarFormulario() {
    $("#studentForm")[0].reset();
}

// Función para registrar estudiante con AJAX
function registrarEstudiante() {

    let studentData = obtenerDatosFormulario(); // Obtiene los datos del formulario

    try {
        studentData = obtenerDatosFormulario(); // Obtiene los datos del formulario
        console.log("📩 Datos a enviar al backend:", studentData);
    } catch (error) {
        console.error("❌ Error en obtenerDatosFormulario():", error);
        return; // Detener la ejecución si hay un error
    }

    $.ajax({
        url: "/Student/Create", // Ajusta el endpoint
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(studentData),
        success: function () {
            mostrarAlerta("success", "Registro exitoso", "Estudiante registrado correctamente.");
            limpiarFormulario();
            cargarEstudiantes(); // Llama la función si tienes una tabla de estudiantes
        },
        error: function (error) {
            console.error("Error en el registro:", error);
            mostrarAlerta("error", "Error", "No se pudo registrar el estudiante.");
        }
    });
}



function loadProfessors() {
    console.log('Cargando profesores...');
    $.ajax({
        url: "/Professor/GetAll",
        type: "GET",
        success: function (data) {
            console.log('Datos recibidos:', data); // Verifica qué se está recibiendo
            $("#professorsTable").empty(); // Vacía la tabla de forma segura

            let tableContent = "";
            data.forEach(professor => {
                tableContent += `
                    <tr>
                        <td>${professor.name}</td>
                        <td>${professor.email}</td>
                        <td>${professor.description}</td>
                    </tr>`;
            });

            $("#professorsTable").html(tableContent);
        },
        error: function () {
            Swal.fire({
                icon: "error",
                title: "Error",
                text: "No se pudo cargar la lista de profesores.",
            });
        }
    });
}
function openProfileModal(id) {
    $.ajax({
        url: "/Professor/GetById?id=" + id,
        type: "GET",
        success: function (professor) {
            console.log("Profesor recibido:", professor);

            $("#profileName").text(professor.name + " " + professor.lastName);
            $("#profileEmail").text(professor.email);
            $("#profileDescription").text(professor.description || "No disponible");
            $("#profileSocialLink").attr("href", professor.socialLink || "#");

            // Si tiene foto, la mostramos
            if (professor.photo) {
                $("#profilePhoto").attr("src", "data:image/png;base64," + arrayBufferToBase64(professor.photo));
            } else {
                $("#profilePhoto").attr("src", "default-avatar.png");
            }

            $("#perfil-modal").modal("show");
        },
        error: function () {
            Swal.fire({
                icon: "error",
                title: "Error",
                text: "No se pudo cargar el perfil del profesor.",
            });
        }
    });
}

// Convierte arrayBuffer a Base64
function arrayBufferToBase64(buffer) {
    let binary = "";
    let bytes = new Uint8Array(buffer);
    for (let i = 0; i < bytes.length; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}

function loadNewsAndComments() {
    console.log("Cargando noticias...");

    $.ajax({
        url: "/News/GetAllNews", 
        type: "GET",
        success: function (data) {
            let newsContent = "";

            if (data.length === 0) {
                newsContent = "<p>No hay noticias disponibles.</p>";
            } else {
                data.forEach(news => {
                    newsContent += `
                        <article class="news-item">
                            <div class="news-content">
                                <h3>${news.title}</h3>
                                <p>${news.textNews}</p>
                                <span class="news-date">${news.dateNews.split(" ")[0]}</span>
                               <br> </br>
                                <button class="view-comments" data-news-id="${news.id}">Ver comentarios</button>
                            </div>
                        </article>
                    `;
                });
            }

            $(".news-scroll").html(newsContent);
        },
        error: function () {
            Swal.fire({
                icon: "error",
                title: "Error",
                text: "No se pudo cargar las noticias.",
            });
        }
    });

    
    // Close the modal when clicking on the close button
    $(".close-btn").on("click", function () {
        $("#commentsModal").hide();
    });

    // Optionally, close the modal if the user clicks outside the modal content
    $(window).on("click", function (event) {
        if ($(event.target).is("#commentsModal")) {
            $("#commentsModal").hide();
        }
    });
}
