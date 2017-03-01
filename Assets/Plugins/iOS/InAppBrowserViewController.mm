//
//  InAppBrowserViewController.m
//  Unity-iPhone
//
//  Created by Piotr on 04/03/16.
//
//
#include "InAppBrowserViewController.h"

@implementation InAppBrowserConfig

+ (InAppBrowserConfig *)defaultDisplayOptions {
    InAppBrowserConfig *displayOptions = [InAppBrowserConfig new];
    displayOptions.pageTitle = nil;
    displayOptions.displayURLAsPageTitle = YES;
    displayOptions.textColor = nil;
    displayOptions.barBackgroundColor = nil;
    displayOptions.backButtonText = @"Back";
    return displayOptions;
}

@end

@implementation InAppBrowserViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    if (!_config) {
        _config = [InAppBrowserConfig defaultDisplayOptions];
    }
    
    UIWebView *webView = [UIWebView new];
    self.webView = webView;
    [self.view addSubview:webView];
    [self configureNavigationBar];
    [webView setTranslatesAutoresizingMaskIntoConstraints:NO];
    [self.view addConstraints:[NSLayoutConstraint
                               constraintsWithVisualFormat:@"H:|-0-[webView]-0-|"
                               options:NSLayoutFormatDirectionLeadingToTrailing
                               metrics:nil
                               views:NSDictionaryOfVariableBindings(webView)]];
    
    [self.view addConstraints:[NSLayoutConstraint
                               constraintsWithVisualFormat:@"V:|-0-[webView]-0-|"
                               options:NSLayoutFormatDirectionLeadingToTrailing
                               metrics:nil
                               views:NSDictionaryOfVariableBindings(webView)]
     ];
    [self startLoadingWebView];
}

- (void)startLoadingWebView {
    _webView.delegate = self;
    NSURLRequest *request = [NSURLRequest requestWithURL:[NSURL URLWithString:_URL]];
    [_webView loadRequest: request];
    
    UIActivityIndicatorView *indicator = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleGray];
    _indicatorView.hidesWhenStopped  = YES;
    self.indicatorView = indicator;
    [self.view addSubview:_indicatorView];
    [_indicatorView setTranslatesAutoresizingMaskIntoConstraints:NO];
    [self.view addConstraints:@[[NSLayoutConstraint constraintWithItem:_indicatorView
                                                            attribute:NSLayoutAttributeCenterX
                                                            relatedBy:NSLayoutRelationEqual
                                                               toItem:self.view
                                                            attribute:NSLayoutAttributeCenterX
                                                           multiplier:1.f constant:0.f],
                               [NSLayoutConstraint constraintWithItem:_indicatorView
                                                            attribute:NSLayoutAttributeCenterY
                                                            relatedBy:NSLayoutRelationEqual
                                                               toItem:self.view
                                                            attribute:NSLayoutAttributeCenterY
                                                           multiplier:1.f constant:0.f]
                                ]
     
     ];
    [_indicatorView startAnimating];
}

- (void)configureNavigationBar {
    UIBarButtonItem* barButton = [[UIBarButtonItem alloc] initWithTitle:_config.backButtonText
                                                                  style:UIBarButtonItemStylePlain
                                                                 target:self
                                                                 action:@selector(backButtonPressed)];
    
    [self.navigationItem setLeftBarButtonItem:barButton];
    
    if (_config.textColor) {
        NSDictionary *titleTextAttrs = [NSDictionary dictionaryWithObject:_config.textColor
                                                                    forKey:NSForegroundColorAttributeName];
        [self.navigationController.navigationBar setTitleTextAttributes: titleTextAttrs];
        [barButton setTitleTextAttributes:titleTextAttrs forState:UIControlStateNormal];
    }

    if (_config.barBackgroundColor) {
        self.navigationController.navigationBar.barTintColor = _config.barBackgroundColor;
        self.navigationController.navigationBar.translucent = NO;
    }
    
    if (_config.displayURLAsPageTitle) {
        NSURL *URL = [NSURL URLWithString:_URL];
        if (URL != nil) {
            self.navigationItem.title = URL.host;
        }

    } else if (_config.pageTitle) {
        self.navigationItem.title = _config.pageTitle;
    }
    
}

- (BOOL)webView:(UIWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType {
    return YES;
}

- (void)webViewDidFinishLoad:(UIWebView *)webView {
    [_indicatorView stopAnimating];
}

- (void)webView:(UIWebView *)webView didFailLoadWithError:(NSError *)error {
    [_indicatorView stopAnimating];
}

- (void)backButtonPressed {
    [self.navigationController.presentingViewController dismissViewControllerAnimated:true completion:nil];
}

@end
