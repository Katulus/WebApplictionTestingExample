@echo off
setlocal
pushd Client\Application\Tests
node ..\..\..\node_modules\protractor\bin\protractor protractor_conf.js
popd