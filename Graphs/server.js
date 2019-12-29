const http = require('http');
const express = require('express');
const cors = require('cors');
const fs = require('fs');
const app = express();
app.use(cors());

app.get('/', (req,res) => {
    logData = GetData();
    res.send(logData)
});


var reg = /([^\n]*):\ *([^,]*),\ *([^,]*),\ *([^,]*),\ *([^,]*)/g;
function GetData() {
    var data = fs.readFileSync("../Assets/Logging/data.log", "utf8")
        .split("\r\n")
        .map(l => reg.exec(l))
        .filter(e => e!= null)
        .map(l => { return {
            gen: l[1],
            max: l[2],
            min: l[3],
            avg: l[4],
            med: l[5]
        }});

    var fitness = fs.readFileSync("../Assets/Logging/fitnesses.log", "utf8")
        .split("\r\n")
        .filter(l => l)
        .map(l => {

            var s = l.split(":");
            
            var gen = s[0];
            var nums = s[1].split(",")

            return { gen, nums }
        })

    return { data, fitness };
}

console.log(GetData())

// Start the server on port 3000
app.listen(3000, 'localhost');
console.log('Node server running on port 3000');