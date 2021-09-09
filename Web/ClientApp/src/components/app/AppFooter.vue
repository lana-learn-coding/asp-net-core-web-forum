<template>
  <v-footer>
    <v-container class="body-2">
      <v-row>
        <v-col cols=12 sm="6" md="3" lg="4">
          <div class="mb-2 font-weight-medium body-1">
            About us
          </div>
          <div class="mb-1">
            <v-icon class="material-icons-outlined mr-2" small>email</v-icon>
            support@drforummail.com
          </div>
          <div class="mb-1">
            <v-icon class="material-icons-outlined mr-2" small>phone</v-icon>
            0915212328, 0915 561 252
          </div>

        </v-col>
        <v-col cols=12 sm="6" md="3" lg="4">
          <div class="mb-2 font-weight-medium body-1">
            Donate us
          </div>
          <div>
            <v-icon class="material-icons-outlined mr-2" small>payment</v-icon>
            <a href="#">Donate via paypal</a>
          </div>
          <div class="mb-1">
            <v-icon class="material-icons-outlined mr-2" small>monetization_on</v-icon>
            <a href="#">Donate using bitcoin</a>
          </div>
        </v-col>
        <v-col cols=12 sm="6" md="3" lg="4">
          <div class="mb-2 font-weight-medium body-1">
            Reference
          </div>
          <div>
            <v-icon class="material-icons-outlined mr-2" small>edit</v-icon>
            Sitdownrightnow2552
          </div>
          <div>
            <v-icon class="material-icons-outlined mr-2" small>check_circle</v-icon>
            License: MIT
          </div>
          <div class="mb-1">
            <v-icon class="material-icons-outlined mr-2" small>format_list_bulleted</v-icon>
            <a href="#">Terms of use</a>
          </div>
        </v-col>
        <v-col cols="12">
          <v-divider class="mb-3"></v-divider>
          <div>
            Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum
            has been the industry's standard dummy text ever since the 1500s, when an unknown
            printer took
          </div>
        </v-col>
      </v-row>
    </v-container>

  </v-footer>
</template>

<script>
export default {
  name: 'AppFooter',
};
</script>
