cd ..

npx create-nx-workspace@latest allors --preset=empty --nxCloud=false

cd allors

npm install -D jest-chain
npm install -D jest-trx-results-processor
npm install -D @nx/angular

npm install @angular/cdk
npm install @angular/material
npm install @angular/material-luxon-adapter
npm install bootstrap@4.6.0
npm install common-tags
npm install cross-fetch
npm install date-fns
npm install easymde
npm install jsnlog
npm install luxon

# Base
# npx nx g @nx/angular:application base/workspace/angular/foundation-app --routing=true --standalone=false --style=css --e2eTestRunner=none --minimal=true --strict=false
# npx nx g @nx/angular:application base/workspace/angular-material/application-app --routing=false --standalone=false --style=scss --e2eTestRunner=none --minimal=true --strict=false
# npx nx g @nx/js:lib base/workspace/angular/foundation --bundler=none --unitTestRunner=none --minimal=true --strict=false
# npx nx g @nx/js:lib base/workspace/angular/application --bundler=none --unitTestRunner=none --minimal=true --strict=false
# npx nx g @nx/js:lib base/workspace/angular-material/foundation --bundler=none --unitTestRunner=none --minimal=true --strict=false
# npx nx g @nx/js:lib base/workspace/angular-material/application --bundler=none --unitTestRunner=none --minimal=true --strict=false
# npx nx g @nx/js:lib base/workspace/domain --bundler=none --unitTestRunner=none --minimal=true --strict=false
# npx nx g @nx/js:lib base/workspace/meta --bundler=none --unitTestRunner=none --minimal=true --strict=false
# npx nx g @nx/js:lib base/workspace/meta-json --bundler=none --unitTestRunner=none --minimal=true --strict=false

# Core
npx nx g @nx/js:lib core/workspace/domain --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib core/workspace/meta --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib core/workspace/meta-json --bundler=none --unitTestRunner=none --minimal=true --strict=false

# System
npx nx g @nx/js:lib system/common/protocol-json --bundler=none --unitTestRunner=none --minimal=true --strict=false

npx nx g @nx/js:lib system/workspace/adapters --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib system/workspace/adapters-tests --bundler=none --unitTestRunner=jest --minimal=true --strict=false
npx nx g @nx/js:lib system/workspace/adapters-json --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib system/workspace/adapters-json-tests --bundler=none --unitTestRunner=jest --minimal=true --strict=false

npx nx g @nx/js:lib system/workspace/domain --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib system/workspace/meta --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib system/workspace/meta-tests --bundler=none --unitTestRunner=jest --minimal=true --strict=false
npx nx g @nx/js:lib system/workspace/meta-json --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib system/workspace/meta-json-tests --bundler=none --unitTestRunner=jest --minimal=true --strict=false
