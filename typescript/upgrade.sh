cd ..

npx create-nx-workspace@latest allors --preset=empty --cli=nx --nx-cloud=false

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

// Base
npx nx g @nx/angular:application base/workspace/angular/foundation-app --routing=true --standalone=false --style=css --e2eTestRunner=none
npx nx g @nx/angular:application base/workspace/angular-material/application-app --routing=falsels --standalone=false --style=scss --e2eTestRunner=none
npx nx g @nx/js:lib base/workspace/angular/foundation --bundler=tsc --unitTestRunner=jest
npx nx g @nx/js:lib base/workspace/angular/application --bundler=tsc --unitTestRunner=jest
npx nx g @nx/js:lib base/workspace/angular-material/foundation --bundler=tsc --unitTestRunner=jest
npx nx g @nx/js:lib base/workspace/angular-material/application --bundler=tsc --unitTestRunner=jest
npx nx g @nx/js:lib base/workspace/domain --bundler=tsc --unitTestRunner=jest
npx nx g @nx/js:lib base/workspace/meta --bundler=tsc --unitTestRunner=jest
npx nx g @nx/js:lib base/workspace/meta-json --bundler=tsc --unitTestRunner=jest

// Core
npx nx g @nx/js:lib core/workspace/domain --bundler=tsc --unitTestRunner=jest
npx nx g @nx/js:lib core/workspace/meta --bundler=tsc --unitTestRunner=jest
npx nx g @nx/js:lib core/workspace/meta-json --bundler=tsc --unitTestRunner=jest

// System
npx nx g @nx/js:lib system/common/protocol-json --bundler=tsc --unitTestRunner=jest

npx nx g @nx/js:lib system/workspace/adapters --bundler=tsc --unitTestRunner=jest
npx nx g @nx/js:lib system/workspace/adapters-tests --bundler=tsc --unitTestRunner=jest
npx nx g @nx/js:lib system/workspace/adapters-json --bundler=tsc --unitTestRunner=jest
npx nx g @nx/js:lib system/workspace/adapters-json-tests --bundler=tsc --unitTestRunner=jest

npx nx g @nx/js:lib system/workspace/domain --bundler=tsc --unitTestRunner=jest
npx nx g @nx/js:lib system/workspace/meta --bundler=tsc --unitTestRunner=jest
npx nx g @nx/js:lib system/workspace/meta-tests --bundler=tsc --unitTestRunner=jest
npx nx g @nx/js:lib system/workspace/meta-json --bundler=tsc --unitTestRunner=jest
npx nx g @nx/js:lib system/workspace/meta-json-tests --bundler=tsc --unitTestRunner=jest 
