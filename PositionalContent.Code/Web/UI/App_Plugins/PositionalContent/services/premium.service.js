 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module('umbraco').factory('HiFi.PositionalContent.PremiumServices', [
    '$injector',
    function ($injector) {

        var _isPremium, _globalControls, _breakpoint, _cropper, _item;

        return {
            isPremium: function () {

                if(_isPremium == undefined)
                    this.init();
                return _isPremium;
            },
            init: function () {
                _isPremium = true;

                $injector.get('HiFi.PositionalContent.Premium.BreakpointService');

                try { _globalControls = $injector.get('HiFi.PositionalContent.Premium.GlobalControlsService'); } catch { _isPremium = false; };
                try { _breakpoint = $injector.get('HiFi.PositionalContent.Premium.BreakpointService'); } catch (ex) { _isPremium = false; };
                try { _cropper = $injector.get('HiFi.PositionalContent.Premium.CropperService'); } catch { _isPremium = false; };
                try { _item = $injector.get('HiFi.PositionalContent.Premium.CropperService'); } catch { _isPremium = false; };
                
            },
            get globalControlsService() {
                return _globalControls;
            },
            get breakpointService() {
                return _breakpoint;
            },
            get cropperService() {
                return _cropper;
            },
            get itemService() {
                return _item;
            }
        };
    }]);