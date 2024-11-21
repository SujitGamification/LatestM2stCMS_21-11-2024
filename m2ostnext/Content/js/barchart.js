
    jQuery.getScript('http://cdnjs.cloudflare.com/ajax/libs/raphael/2.1.0/raphael-min.js', function () {
        jQuery.getScript('http://cdnjs.cloudflare.com/ajax/libs/morris.js/0.5.0/morris.min.js', function () {

            //Morris.Line({
            //    element: 'line-example',
            //    data: [
            //      { year: '2010', value: 20 },
            //      { year: '2011', value: 10 },
            //      { year: '2012', value: 5 },
            //      { year: '2013', value: 2 },
            //      { year: '2014', value: 20 }
            //    ],
            //    xkey: 'year',
            //    ykeys: ['value'],
            //    labels: ['Value']
            //});

         

            Morris.Bar({
                element: 'bar-example',
                data: [
                   { y: 'Category', a: 25, b: 10 },
                   { y: 'Questions', a: 35, b: 30 },
                   { y: 'Answers', a: 20, b: 10 }
                   //{ y: 'Apr 2014', a: 75, b: 65 },
                   //{ y: 'May 2014', a: 50, b: 40 },
                   //{ y: 'Jun 2014', a: 75, b: 65 }
                ],
                xkey: 'y',
                ykeys: ['a', 'b'],
                labels: ['Approved', 'Not Approved']
            });

            //Morris.Area({
            //    element: 'area-example',
            //    data: [
            //      { y: '1.1', a: 0, b: 90 },
            //      { y: '2.1', a: 10, b: 65 },
            //      { y: '3.1', a: 25, b: 40 },
            //      { y: '4.1', a: 30, b: 65 },
            //      { y: '5.1', a: 20, b: 40 },
            //      { y: '6.1', a: 35, b: 65 },
            //      { y: '7.1', a: 25, b: 90 }
            //    ],
            //    xkey: 'y',
            //    ykeys: ['a'],
            //    labels: ['Series A']
            //}
            //);

            //Morris.Area({
            //    element: 'area-example1',
            //    data: [
            //      { y: '1.1.', a: 0, b: 90 },
            //      { y: '2.1.', a: 10, b: 65 },
            //      { y: '3.1.', a: 20, b: 40 },
            //      { y: '4.1.', a: 15, b: 65 },
            //      { y: '5.1.', a: 25, b: 40 },
            //      { y: '6.1.', a: 35, b: 65 },
            //      { y: '7.1.', a: 50, b: 90 }
            //    ],
            //    xkey: 'y',
            //    ykeys: ['a'],
            //    labels: ['Series A']
            //});

            //Morris.Area({
            //    element: 'area-example2',
            //    data: [
            //      { y: '1.1.', a: 0, b: 90 },
            //      { y: '2.1.', a: 25, b: 65 },
            //      { y: '3.1.', a: 50, b: 40 },
            //      { y: '4.1.', a: 75, b: 65 },
            //      { y: '5.1.', a: 50, b: 40 },
            //      { y: '6.1.', a: 75, b: 65 },
            //      { y: '7.1.', a: 50, b: 90 }
            //    ],
            //    xkey: 'y',
            //    ykeys: ['a'],
            //    labels: ['Series A']
            //});
            //Morris.Area({
            //    element: 'area-example3',
            //    data: [
            //      { y: '1.1.', a: 0, b: 90 },
            //      { y: '2.1.', a: 35, b: 65 },
            //      { y: '3.1.', a: 50, b: 40 },
            //      { y: '4.1.', a: 25, b: 65 },
            //      { y: '5.1.', a: 20, b: 40 },
            //      { y: '6.1.', a: 30, b: 65 },
            //      { y: '7.1.', a: 30, b: 90 }
            //    ],
            //    xkey: 'y',
            //    ykeys: ['a'],
            //    labels: ['Series A']
            //});

            Morris.Donut({
                element: 'donut-example',
                data: [
                 { label: "Android", value: 60 },
                 { label: "iPhone", value: 30 },
                 { label: "Web", value: 10 }
                ]
            });
            Morris.Donut({
                element: 'donut-example1',
                data: [
                 { label: "Web", value: 70 },
                 { label: "Android", value: 20 },
                 { label: "iPhone", value: 10 }
                ]
            });
            Morris.Donut({
                element: 'donut-example2',
                data: [
                  { label: "Web", value: 60 },
                 { label: "Android", value: 30 },
                 { label: "iPhone", value: 10 }
                ]
            });
            Morris.Donut({
                element: 'donut-example3',
                data: [
                 { label: "iPhone", value: 60 },
                 { label: "Android", value: 30 },
                 { label: "Web", value: 10 }
                ]
            });


        });
    });

