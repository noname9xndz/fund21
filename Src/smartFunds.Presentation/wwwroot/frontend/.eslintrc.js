module.exports = {
    "extends": "airbnb-base",
    "env": {
        "browser": true,
        "node": true,
        "jasmine": true,
        "jquery": true
    },
    "globals": {
        "$zopim": true,
        "dataLayer": true,
        "google": true,
        "autocompleteService": true,
        "geocoder": true,
        "geocore": true,
        "YT": true,
    },
    "rules": {
        "prefer-destructuring": ["error", {
          "array": true,
          "object": true
        }, {
          "enforceForRenamedProperties": false
        }],
        "linebreak-style": 0,
        "comma-dangle": ["error", "never"]
    }
};