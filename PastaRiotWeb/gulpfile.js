var gulp = require("gulp"),
    sass = require("gulp-sass"),
    concat = require("gulp-concat"),
    cleanCss = require("gulp-clean-css");

gulp.task("sass", function () {
    return gulp.src('./scss/*.scss')
        .pipe(sass())
        .pipe(cleanCss({ compatibility: 'ie8' }))
        .pipe(gulp.dest('./wwwroot/css/'));
});