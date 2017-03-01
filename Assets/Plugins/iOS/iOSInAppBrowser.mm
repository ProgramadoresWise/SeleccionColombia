//
//  iOSInAppBrowser.m
//
//  Created by Piotr Zmudzinski on 04/03/16.
//  contact: ptr.zmudzinski@gmail.com
//

#import <Foundation/Foundation.h>
#import "UnityAppController.h"
#import "InAppBrowserViewController.h"

extern void UnitySendMessage(const char *, const char *, const char *);

struct DisplayOptions {
    char *pageTitle;
    char *backButtonText;
    char *barBackgroundColor;
    char *textColor;
    bool displayURLAsPageTitle;
};

@interface UIColor(HexString)

+ (CGFloat) colorComponentFrom:(NSString *) string start:(NSUInteger) start length:(NSUInteger) length;

@end


@implementation UIColor(HexString)

+ (UIColor *) colorWithHexChar: (char *) hexChar {
    return [UIColor colorWithHexString:[NSString stringWithUTF8String:hexChar]];
}

+ (UIColor *) colorWithHexString: (NSString *) hexString {
    NSString *colorString = [[hexString stringByReplacingOccurrencesOfString: @"#" withString: @""] uppercaseString];
    CGFloat alpha, red, blue, green;
    switch ([colorString length]) {
        case 3: // #RGB
            alpha = 1.0f;
            red   = [self colorComponentFrom: colorString start: 0 length: 1];
            green = [self colorComponentFrom: colorString start: 1 length: 1];
            blue  = [self colorComponentFrom: colorString start: 2 length: 1];
            break;
        case 4: // #ARGB
            alpha = [self colorComponentFrom: colorString start: 0 length: 1];
            red   = [self colorComponentFrom: colorString start: 1 length: 1];
            green = [self colorComponentFrom: colorString start: 2 length: 1];
            blue  = [self colorComponentFrom: colorString start: 3 length: 1];
            break;
        case 6: // #RRGGBB
            alpha = 1.0f;
            red   = [self colorComponentFrom: colorString start: 0 length: 2];
            green = [self colorComponentFrom: colorString start: 2 length: 2];
            blue  = [self colorComponentFrom: colorString start: 4 length: 2];
            break;
        case 8: // #AARRGGBB
            alpha = [self colorComponentFrom: colorString start: 0 length: 2];
            red   = [self colorComponentFrom: colorString start: 2 length: 2];
            green = [self colorComponentFrom: colorString start: 4 length: 2];
            blue  = [self colorComponentFrom: colorString start: 6 length: 2];
            break;
        default:
            return nil;
    }
    return [UIColor colorWithRed: red green: green blue: blue alpha: alpha];
}

+ (CGFloat) colorComponentFrom: (NSString *) string start: (NSUInteger) start length: (NSUInteger) length {
    NSString *substring = [string substringWithRange: NSMakeRange(start, length)];
    NSString *fullHex = length == 2 ? substring : [NSString stringWithFormat: @"%@%@", substring, substring];
    unsigned hexComponent;
    [[NSScanner scannerWithString: fullHex] scanHexInt: &hexComponent];
    return hexComponent / 255.0;
}

@end


@interface InAppBrowserConfig(DisplayOptionsStruct)

+ (InAppBrowserConfig*)fromDisplayOptions:(struct DisplayOptions)options;

@end

@implementation InAppBrowserConfig(DisplayOptionsStruct)

+ (InAppBrowserConfig*)fromDisplayOptions:(struct DisplayOptions)options {
    InAppBrowserConfig *config = [InAppBrowserConfig defaultDisplayOptions];
    
    if (options.pageTitle != NULL) {
        config.pageTitle = [NSString stringWithUTF8String:options.pageTitle];
    }
    
    if (options.backButtonText != NULL) {
        config.backButtonText = [NSString stringWithUTF8String:options.backButtonText];
    }
    
    if (options.barBackgroundColor != NULL) {
        config.barBackgroundColor = [UIColor colorWithHexChar:options.barBackgroundColor];
    }
    
    if (options.textColor != NULL) {
        config.textColor = [UIColor colorWithHexChar:options.textColor];
    }
    
    config.displayURLAsPageTitle = options.displayURLAsPageTitle;
    
    return config;
}

@end


extern "C" {
    
    void _OpenInAppBrowser(char *URL, struct DisplayOptions displayOptions) {
        NSString *urlAsString = [NSString stringWithUTF8String:URL];
        UnityAppController *unityAppController = GetAppController();
        InAppBrowserViewController *vc = [InAppBrowserViewController new];
        vc.URL = urlAsString;
        vc.config = [InAppBrowserConfig fromDisplayOptions: displayOptions];
        UINavigationController *navigationController = [[UINavigationController alloc] initWithRootViewController:vc];
        [unityAppController.rootViewController presentViewController:navigationController animated:true completion:nil];
    }

}

