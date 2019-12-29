const http = new XMLHttpRequest()

setInterval(() => {
    http.open("GET", "http:localhost:3000")
    http.send()
    http.onload = () => updateGraphs(JSON.parse(http.responseText))
}, 1000)

var ctx = { 
    data: document.getElementById('dataChart').getContext('2d'),
    fitness: document.getElementById('fitnessChart').getContext('2d')
};

var colors = [
    'rgba(255, 99, 132, 1)',
    'rgba(54, 162, 235, 1)',
    'rgba(255, 206, 86, 1)',
    'rgba(75, 192, 192, 1)',
    'rgba(153, 102, 255, 1)',
    'rgba(255, 159, 64, 1)',
];

function lerp(a,b,t) { return a * t + b * (1-t);}

function getColor(t) {
    return 'rgba(' + lerp(255, 0, t) + ", " + lerp(0, 255, t) + ", 0)"
}

var charts;

function updateGraphs(responce) {
    var data = responce.data;
    var fitness = responce.fitness;


    var size = fitness[0].length;


    
    var relativeFitnesses = [];


    var relativeFitnesses = fitness.map(f => {
        var nums = f.nums;
        var max = Math.max(...nums);
        var min = Math.min(...nums);

        return nums.map(n => (n-min)/(max - min));
    })

    if (!charts) {
        charts = {
            data: new Chart(ctx.data, {
                type: 'line',
                data: {
                    labels: data.map(d => d.gen),
                    datasets: [
                        {
                            label: 'Maximum',
                            data: data.map(d => d.max),
                            backgroundColor: colors[0],
                            borderColor: colors[0],
                            borderWidth: 1,
                            fill: false
                        },
                        {
                            label: 'Minimum',
                            data: data.map(d => d.min),
                            backgroundColor: colors[1],
                            borderColor: colors[1],
                            borderWidth: 1,
                            fill: false
                        },
                        {
                            label: 'Average',
                            data: data.map(d => d.avg),
                            backgroundColor: colors[2],
                            borderColor: colors[2],
                            borderWidth: 1,
                            fill: false
                        },
                        {
                            label: 'Median',
                            data: data.map(d => d.med),
                            backgroundColor: colors[3],
                            borderColor: colors[3],
                            borderWidth: 1,
                            fill: false
                        }
                    ]
                },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'Chart.js Line Chart'
                    },
                    tooltips: {
                        mode: 'index',
                        intersect: false,
                    },
                    hover: {
                        mode: 'nearest',
                        intersect: true
                    },
                    scales: {
                        xAxes: [{
                            display: true,
                            scaleLabel: {
                                display: true,
                                labelString: 'Month'
                            }
                        }],
                        yAxes: [{
                            display: true,
                            scaleLabel: {
                                display: true,
                                labelString: 'Value'
                            }
                        }]
                    }
                }
            })
        //     fitness: new Chart(ctx.fitness, {
        //         type: 'bar',
        //         data: {
        //             labels: fitness.map(d => d.gen),
        //             datasets: relativeFitnesses.map((rf, index) => {
        //                 return {
        //                     label: index,
        //                     data: rf,
        //                     backgroundColor: getColor(index/relativeFitnesses.length)
        //                 }
        //             })
        //         },
        //         options: {
        //             responsive: true,
        //             title: {
        //                 display: true,
        //                 text: 'Chart.js Line Chart'
        //             },
        //             tooltips: {
        //                 mode: 'index',
        //                 intersect: false,
        //             },
        //             hover: {
        //                 mode: 'nearest',
        //                 intersect: true
        //             },
        //             scales: {
        //                 xAxes: [{
        //                     display: true,
        //                     scaleLabel: {
        //                         display: true,
        //                         labelString: 'Month'
        //                     }
        //                 }],
        //                 yAxes: [{
        //                     display: true,
        //                     scaleLabel: {
        //                         display: true,
        //                         labelString: 'Value'
        //                     }
        //                 }]
        //             }
        //         }
        //     })
        };
    }

    charts.data.data.labels = data.map(d => d.gen);
    charts.data.data.datasets[0].data = data.map(d => d.max);
    charts.data.data.datasets[1].data = data.map(d => d.min);
    charts.data.data.datasets[2].data = data.map(d => d.avg);
    charts.data.data.datasets[3].data = data.map(d => d.med);
    charts.data.update();

    // if (charts.fitness.data.labels.length == fitness.length) return;
    
    // charts.fitness.data.labels = fitness.map(d => d.gen);
    // charts.fitness.data.datasets = relativeFitnesses.map((rf, index) => {
    //         return {
    //             label: index,
    //             data: rf,
    //             backgroundColor: getColor(index/relativeFitnesses.length)
    //         }
    //     });

    
    // charts.fitness.update();
}
