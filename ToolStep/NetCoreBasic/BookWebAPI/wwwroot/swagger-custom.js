window.onload = function () {
    // Find the Swagger UI container element
    var swaggerUiContainer = document.getElementsByClassName('swagger-ui')[0];

    // Create the "Generate Word" button
    var generateWordButton = document.createElement('button');
    generateWordButton.innerHTML = 'Generate Word';
    generateWordButton.className = 'download-word-button';
    generateWordButton.onclick = function () {
        // Generate the Word document
        window.location.href = '/swagger/api-docs/word';
    };

    // Add the button to the Swagger UI container
    swaggerUiContainer.prepend(generateWordButton);
};
