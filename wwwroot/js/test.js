const videoEl = document.getElementById("camara")

navigator.mediaDevices.getUserMedia({
    video: {
        width: { min: 480, ideal: 1080 },
        height: { min: 480, ideal: 1080 },
        aspectRatio: { ideal: 1 },
        facingMode: 'environment',
    }
})
.then((stream) => {
    localStream = stream
    videoEl.srcObject = stream

    // get the active track of the stream
    const track = stream.getVideoTracks()[0]
    console.log(track.getSettings())

    // get current camera capabilities
    let capabilities = track.getCapabilities()
    console.log(capabilities)

    // update a capability
    // if (capabilities.brightness) {
    //     track.applyConstraints({
    //         advanced: [{
    //             brightness: 0
    //         }]
    //     })
    //     .catch(error => console.error("Error trying to apply constraints: ", error))
    // }
})

function capture() {
    var canvas = document.getElementById("canvas");
    var video = document.getElementById("camara");
    document.getElementById('codigo').textContent = "Decodificando..."

    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    canvas
      .getContext("2d")
      .drawImage(video, 0, 0, video.videoWidth, video.videoHeight);

    image = canvas.toDataURL("image/png").split(';base64,')[1]

    let formData = new FormData()
    formData.append("image", image)

    fetch('/Home/Index', {
        method: 'post',
        body: formData
    })
    .then(response => {
        if (response.status == 200) {
            return response.json()
        }
    })
    .then(data => {
        console.log(data)
        if (data.responseCode == 0) {
            document.getElementById('codigo').textContent = data.text
            document.getElementById('formato').textContent = data.format
            document.getElementById('tiempo').textContent = data.timeElapse
        } else if (data.responseCode == 1) {
            document.getElementById('codigo').textContent = "No se detecto ningun codigo de barras"
            document.getElementById('formato').textContent = ""
            document.getElementById('tiempo').textContent = ""
        } else {
            alert("Ocurrio un error inesperado")
        }
    })
    .catch(error => {
        console.error(error)
    })
  }

  function stopCamera() {
    try {
        if (localStream) {
            localStream.getTracks().forEach(track => track.stop());
        }
    } catch (e){
        alert(e.message);
    }
}