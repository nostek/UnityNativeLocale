#import <Foundation/Foundation.h>

@interface NativeLocale : NSObject
{
    
}

- (id)init;
- (NSString *)getLanguage;
- (NSString *)getCountryCode;

@end

@implementation NativeLocale

- (id)init
{
    self = [super init];
    return self;
}

- (NSString *)getLanguage
{
    id langs = [NSLocale preferredLanguages];
    if(langs != nil && [langs count] > 0)
        return [langs objectAtIndex:0];
    return [[NSLocale currentLocale] objectForKey:NSLocaleLanguageCode];
}

- (NSString *)getCountryCode
{
    NSString *cc = [[NSLocale currentLocale] objectForKey:NSLocaleCountryCode];
    return cc;
}

@end


// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

// Helper method to create C string copy
char* MakeStringCopy (const char* string)
{
	if (string == NULL)
		return NULL;
	
	char* res = (char*)malloc(strlen(string) + 1);
	strcpy(res, string);
	return res;
}

static NativeLocale* instance = nil;

extern "C" {

	const char* _CNativeLocaleGetLanguage()
	{
        if(instance == nil)
            instance = [[NativeLocale alloc] init];
        
		return MakeStringCopy([[instance getLanguage] UTF8String]);
	}

	const char* _CNativeLocaleGetCountryCode()
	{
		if(instance == nil)
			instance = [[NativeLocale alloc] init];

		return MakeStringCopy([[instance getCountryCode] UTF8String]);
	}
    
}
