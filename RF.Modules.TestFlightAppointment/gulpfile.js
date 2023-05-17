/******************************************************************************
 *
 *   Copyright Corvinus University of Budapest, Budapest, HUN.
 *
 * ---------------------------------------------------------------------------
 *   This is a part of the RF.Modules.TestFlightAppointment project
 *
 *****************************************************************************/

var gulp = require("gulp");
var sass = require("gulp-sass")(require('sass'));
var strip = require("gulp-strip-css-comments");
var concat = require('gulp-concat');

gulp.task('module-styles', function () {
    return gulp
        .src('Assets/Styles/module.scss')
        .pipe(sass())
        .pipe(strip())
        .pipe(gulp.dest('./'));
});

gulp.task('module-scripts', function () {
    var scripts = [
        'Assets/Scripts/Dialog.js',
        'Assets/Scripts/BookingGridProxy.js',
        'Assets/Scripts/BookingGridPassengerRow.js',
        'Assets/Scripts/BookingGridForm.js'
    ];

    return gulp
        .src(scripts)
        .pipe(concat('BookingGrid.js'))
        .pipe(gulp.dest('Scripts'));
})

gulp.task('build', gulp.parallel('module-styles', 'module-scripts'));
